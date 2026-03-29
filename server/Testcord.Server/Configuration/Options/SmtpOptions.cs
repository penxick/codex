using System.ComponentModel.DataAnnotations;

namespace Testcord.Server.Configuration.Options;

public sealed class SmtpOptions
{
    public const string SectionName = "Smtp";

    [Required]
    public string Host { get; set; } = "localhost";

    [Range(1, 65535)]
    public int Port { get; set; } = 1025;

    public string? UserName { get; set; }

    public string? Password { get; set; }

    [Required]
    [EmailAddress]
    public string FromEmail { get; set; } = "noreply@testcord.local";

    [Required]
    public string FromName { get; set; } = "Testcord";

    public bool UseSsl { get; set; }
}
