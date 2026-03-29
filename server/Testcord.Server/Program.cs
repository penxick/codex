using Microsoft.AspNetCore.Authentication.JwtBearer;
using Testcord.Server.Application;
using Testcord.Server.Configuration.Options;
using Testcord.Server.Infrastructure;
using Testcord.Server.Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "TESTCORD_");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services
    .AddOptions<SmtpOptions>()
    .Bind(builder.Configuration.GetSection(SmtpOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var databaseInitializer = scope.ServiceProvider.GetRequiredService<Testcord.Server.Infrastructure.Persistence.DatabaseInitializer>();
        await databaseInitializer.InitializeAsync();
    }
    catch (Exception exception)
    {
        logger.LogWarning(exception, "Database initialization was skipped because the local PostgreSQL server is unavailable or not yet configured.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok("healthy"));
app.MapGet("/api", () => Results.Redirect("/api/bootstrap"));
app.MapHub<PresenceHub>("/hubs/presence");
app.MapHub<MessagingHub>("/hubs/messaging");
app.MapGet("/", () => Results.Redirect(app.Environment.IsDevelopment() ? "/swagger" : "/health"));

app.Run();

public partial class Program;
