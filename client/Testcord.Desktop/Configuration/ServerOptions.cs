namespace Testcord.Desktop.Configuration;

public sealed class ServerOptions
{
    public const string SectionName = "Server";

    public string BaseUrl { get; set; } = "https://localhost:7217";
}
