namespace Testcord.Server.Domain.Entities;

public sealed class Channel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ServerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Text";
    public int Position { get; set; }
}
