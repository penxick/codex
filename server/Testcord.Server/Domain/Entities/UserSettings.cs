namespace Testcord.Server.Domain.Entities;

public sealed class UserSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Theme { get; set; } = "Dark";
    public string? MicrophoneDeviceId { get; set; }
    public string? SpeakerDeviceId { get; set; }
    public int InputVolume { get; set; } = 100;
    public int OutputVolume { get; set; } = 100;
}
