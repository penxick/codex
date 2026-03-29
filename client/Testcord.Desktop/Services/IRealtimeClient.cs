namespace Testcord.Desktop.Services;

public interface IRealtimeClient
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
