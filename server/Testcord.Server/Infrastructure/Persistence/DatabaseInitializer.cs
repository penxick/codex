using Microsoft.EntityFrameworkCore;

namespace Testcord.Server.Infrastructure.Persistence;

public sealed class DatabaseInitializer
{
    private readonly TestcordDbContext _dbContext;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(TestcordDbContext dbContext, ILogger<DatabaseInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Applying pending EF Core migrations for Testcord.");
        await _dbContext.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("Database migration step completed successfully.");
    }
}
