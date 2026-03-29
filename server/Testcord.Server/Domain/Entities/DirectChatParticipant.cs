namespace Testcord.Server.Domain.Entities;

public sealed class DirectChatParticipant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DirectChatId { get; set; }
    public Guid UserId { get; set; }
}
