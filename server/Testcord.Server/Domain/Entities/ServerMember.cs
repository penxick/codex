namespace Testcord.Server.Domain.Entities;

public sealed class ServerMember
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ServerId { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; } = "Member";
    public DateTimeOffset JoinedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
