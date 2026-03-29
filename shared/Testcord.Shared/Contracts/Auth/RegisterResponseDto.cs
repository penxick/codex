namespace Testcord.Shared.Contracts.Auth;

public sealed record RegisterResponseDto(Guid UserId, string Email, string Nickname, string DisplayName, bool RequiresEmailVerification);
