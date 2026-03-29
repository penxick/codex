namespace Testcord.Shared.Contracts.Auth;

public sealed record VerifyEmailRequestDto(string Email, string Code);
