namespace Testcord.Shared.Contracts.Auth;

public sealed record VerifyEmailResponseDto(Guid UserId, bool IsEmailConfirmed);
