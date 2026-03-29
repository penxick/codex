using Testcord.Shared.Contracts.Auth;

namespace Testcord.Server.Application.Auth;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<VerifyEmailResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthSessionDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthSessionDto> RefreshAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}
