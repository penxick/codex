namespace Testcord.Shared.Contracts.Messaging;

public sealed record MessageDto(
    Guid Id,
    Guid AuthorId,
    string AuthorDisplayName,
    string Content,
    DateTimeOffset CreatedAtUtc,
    bool IsEdited);
