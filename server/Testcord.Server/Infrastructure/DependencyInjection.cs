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
            configuration.GetConnectionString("MySql")
            ?? "Server=localhost;Port=3306;Database=testcord;User=testcord;Password=testcord;";
        var serverVersion = new MySqlServerVersion(new Version(8, 4, 0));

        services.AddDbContext<TestcordDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });

        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}
