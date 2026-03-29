namespace Testcord.Server.Domain.Entities;

public sealed class CallSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? DirectChatId { get; set; }
    public Guid? ChannelId { get; set; }
    public string Mode { get; set; } = "OneToOne";
    public string State { get; set; } = "Idle";
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
