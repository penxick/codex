using Microsoft.AspNetCore.Mvc;
using Testcord.Server.Application.Auth;
using Testcord.Shared.Contracts.Auth;
using Testcord.Shared.Contracts.Common;

namespace Testcord.Server.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<RegisterResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(new ApiResponse<RegisterResponseDto>(true, response));
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new ApiResponse<RegisterResponseDto>(false, null, exception.Message));
        }
    }

    [HttpPost("verify-email")]
    [ProducesResponseType(typeof(ApiResponse<VerifyEmailResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyEmail(VerifyEmailRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.VerifyEmailAsync(request, cancellationToken);
            return Ok(new ApiResponse<VerifyEmailResponseDto>(true, response));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ApiResponse<VerifyEmailResponseDto>(false, null, exception.Message));
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthSessionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.LoginAsync(request, cancellationToken);
            return Ok(new ApiResponse<AuthSessionDto>(true, response));
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new ApiResponse<AuthSessionDto>(false, null, exception.Message));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ApiResponse<AuthSessionDto>(false, null, exception.Message));
        }
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthSessionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh(RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.RefreshAsync(request, cancellationToken);
            return Ok(new ApiResponse<AuthSessionDto>(true, response));
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new ApiResponse<AuthSessionDto>(false, null, exception.Message));
        }
    }
}
