using System.ComponentModel.DataAnnotations;

namespace Testcord.Server.Configuration.Options;

public sealed class MySqlOptions
{
    public const string SectionName = "MySql";

    [Required]
    public string ServerVersion { get; set; } = "8.4.0";
}
