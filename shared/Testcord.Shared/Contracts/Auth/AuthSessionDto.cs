namespace Testcord.Shared.Contracts.Auth;

public sealed record AuthSessionDto(
    Guid UserId,
    string Email,
    string Nickname,
    string DisplayName,
    bool IsEmailConfirmed,
    SessionTokensDto Tokens);
