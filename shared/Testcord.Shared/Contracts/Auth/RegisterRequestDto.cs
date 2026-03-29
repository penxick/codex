namespace Testcord.Shared.Contracts.Auth;

public sealed record RegisterRequestDto(string Email, string Password, string Nickname, string DisplayName);
