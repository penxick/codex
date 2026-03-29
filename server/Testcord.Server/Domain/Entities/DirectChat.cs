namespace Testcord.Server.Domain.Entities;

public sealed class DirectChat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
