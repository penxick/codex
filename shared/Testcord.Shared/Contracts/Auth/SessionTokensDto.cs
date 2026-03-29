namespace Testcord.Shared.Contracts.Auth;

public sealed record SessionTokensDto(string AccessToken, string RefreshToken, DateTimeOffset ExpiresAtUtc);
