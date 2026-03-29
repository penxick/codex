using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Testcord.Server.Configuration.Options;
using Testcord.Server.Domain.Entities;
using Testcord.Server.Infrastructure.Email;
using Testcord.Server.Infrastructure.Identity;
using Testcord.Server.Infrastructure.Persistence;
using Testcord.Shared.Contracts.Auth;

namespace Testcord.Server.Application.Auth;

public sealed class AuthService : IAuthService
{
    private readonly TestcordDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISystemClock _systemClock;
    private readonly IVerificationCodeGenerator _verificationCodeGenerator;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        TestcordDbContext dbContext,
        IEmailSender emailSender,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher,
        ISystemClock systemClock,
        IVerificationCodeGenerator verificationCodeGenerator,
        IOptions<JwtOptions> jwtOptions)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
        _systemClock = systemClock;
        _verificationCodeGenerator = verificationCodeGenerator;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var normalizedNickname = request.Nickname.Trim();

        if (await _dbContext.Users.AnyAsync(x => x.Email == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        if (await _dbContext.Users.AnyAsync(x => x.Nickname == normalizedNickname, cancellationToken))
        {
            throw new InvalidOperationException("Nickname is already in use.");
        }

        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Nickname = normalizedNickname,
            DisplayName = request.DisplayName.Trim()
        };

        _dbContext.Users.Add(user);
        await IssueEmailVerificationCodeAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterResponseDto(user.Id, user.Email, user.Nickname, user.DisplayName, RequiresEmailVerification: true);
    }

    public async Task<VerifyEmailResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var codeHash = HashToken(request.Code.Trim());
        var now = _systemClock.UtcNow;

        var verification = await _dbContext.EmailVerificationCodes
            .Include(x => x.User)
            .Where(x =>
                x.Email == normalizedEmail &&
                x.CodeHash == codeHash &&
                x.ConsumedAtUtc == null &&
                x.ExpiresAtUtc >= now)
            .OrderByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (verification is null)
        {
            throw new InvalidOperationException("Verification code is invalid or expired.");
        }

        verification.ConsumedAtUtc = now;
        verification.User!.IsEmailConfirmed = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new VerifyEmailResponseDto(verification.UserId, verification.User.IsEmailConfirmed);
    }

    public async Task<AuthSessionDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email or password is invalid.");
        }

        if (!user.IsEmailConfirmed)
        {
            throw new InvalidOperationException("Email is not verified.");
        }

        var tokens = _jwtTokenService.CreateTokens(user);
        await SaveRefreshTokenAsync(user.Id, tokens.RefreshToken, cancellationToken);

        return new AuthSessionDto(user.Id, user.Email, user.Nickname, user.DisplayName, user.IsEmailConfirmed, tokens);
    }

    public async Task<AuthSessionDto> RefreshAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var now = _systemClock.UtcNow;
        var tokenHash = HashToken(request.RefreshToken);

        var session = await _dbContext.RefreshSessions
            .Where(x =>
                x.TokenHash == tokenHash &&
                x.RevokedAtUtc == null &&
                x.ExpiresAtUtc >= now)
            .OrderByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (session is null)
        {
            throw new UnauthorizedAccessException("Refresh token is invalid or expired.");
        }

        var user = await _dbContext.Users.FirstAsync(x => x.Id == session.UserId, cancellationToken);
        var tokens = _jwtTokenService.CreateTokens(user);
        session.RevokedAtUtc = now;
        session.ReplacedByTokenHash = HashToken(tokens.RefreshToken);

        _dbContext.RefreshSessions.Add(new RefreshSession
        {
            UserId = user.Id,
            TokenHash = session.ReplacedByTokenHash,
            ExpiresAtUtc = now.AddDays(_jwtOptions.RefreshTokenDays)
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthSessionDto(user.Id, user.Email, user.Nickname, user.DisplayName, user.IsEmailConfirmed, tokens);
    }

    private async Task IssueEmailVerificationCodeAsync(User user, CancellationToken cancellationToken)
    {
        var now = _systemClock.UtcNow;
        var code = _verificationCodeGenerator.Generate();

        await _dbContext.EmailVerificationCodes
            .Where(x => x.UserId == user.Id && x.ConsumedAtUtc == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ExpiresAtUtc, now.AddMinutes(-1)),
                cancellationToken);

        _dbContext.EmailVerificationCodes.Add(new EmailVerificationCode
        {
            User = user,
            Email = user.Email,
            CodeHash = HashToken(code),
            ExpiresAtUtc = now.AddMinutes(15)
        });

        await _emailSender.SendAsync(
            user.Email,
            "Testcord email verification code",
            $"Your verification code is <strong>{code}</strong>.",
            cancellationToken);
    }

    private async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var now = _systemClock.UtcNow;
        _dbContext.RefreshSessions.Add(new RefreshSession
        {
            UserId = userId,
            TokenHash = HashToken(refreshToken),
            ExpiresAtUtc = now.AddDays(_jwtOptions.RefreshTokenDays)
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string HashToken(string value)
    {
        var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}
