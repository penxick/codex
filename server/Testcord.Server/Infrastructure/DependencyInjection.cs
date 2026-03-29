using Microsoft.EntityFrameworkCore;
using Testcord.Server.Infrastructure.Email;
using Testcord.Server.Infrastructure.Identity;
using Testcord.Server.Infrastructure.Persistence;

namespace Testcord.Server.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=testcord;Username=postgres;Password=PUT_PASSWORD_HERE";

        services.AddDbContext<TestcordDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<DatabaseInitializer>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}
