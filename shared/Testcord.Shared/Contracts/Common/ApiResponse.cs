namespace Testcord.Shared.Contracts.Common;

public sealed record ApiResponse<T>(bool Success, T? Data, string? Error = null);
