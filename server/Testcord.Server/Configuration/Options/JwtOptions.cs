using System.ComponentModel.DataAnnotations;

namespace Testcord.Server.Configuration.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Issuer { get; set; } = "Testcord.Server";

    [Required]
    public string Audience { get; set; } = "Testcord.Desktop";

    [Required]
    [MinLength(32)]
    public string SigningKey { get; set; } = "PUT_A_LONG_RANDOM_SIGNING_KEY_HERE";

    [Range(1, 1440)]
    public int AccessTokenMinutes { get; set; } = 60;

    [Range(1, 90)]
    public int RefreshTokenDays { get; set; } = 30;
}
