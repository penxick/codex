using Microsoft.Extensions.DependencyInjection;
using Testcord.Server.Application.Auth;

namespace Testcord.Server.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IVerificationCodeGenerator, VerificationCodeGenerator>();

        return services;
    }
}
