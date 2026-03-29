namespace Testcord.Server.Infrastructure.Identity;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}
