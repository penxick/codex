using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Testcord.Desktop.Services;

public sealed class RealtimeClient : IRealtimeClient
{
    private readonly IBackendClient _backendClient;
    private readonly ILogger<RealtimeClient> _logger;
    private HubConnection? _presenceConnection;

    public RealtimeClient(IBackendClient backendClient, ILogger<RealtimeClient> logger)
    {
        _backendClient = backendClient;
        _logger = logger;
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _presenceConnection ??= new HubConnectionBuilder()
            .WithUrl(new Uri(_backendClient.BaseUri, "/hubs/presence"))
            .WithAutomaticReconnect()
            .Build();

        _logger.LogInformation("Realtime client prepared for {BaseUri}", _backendClient.BaseUri);
        return Task.CompletedTask;
    }
}
