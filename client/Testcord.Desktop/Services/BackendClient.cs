using System.Net.Http;
using Microsoft.Extensions.Options;
using Testcord.Desktop.Configuration;

namespace Testcord.Desktop.Services;

public sealed class BackendClient : IBackendClient
{
    public BackendClient(HttpClient httpClient, IOptions<ServerOptions> options)
    {
        BaseUri = new Uri(options.Value.BaseUrl);
        httpClient.BaseAddress = BaseUri;
    }

    public Uri BaseUri { get; }
}
