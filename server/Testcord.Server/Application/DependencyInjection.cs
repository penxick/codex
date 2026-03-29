using Microsoft.Extensions.DependencyInjection;

namespace Testcord.Server.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
