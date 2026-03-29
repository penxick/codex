using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Testcord.Server.Infrastructure.Persistence;

public sealed class TestcordDbContextFactory : IDesignTimeDbContextFactory<TestcordDbContext>
{
    public TestcordDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=localhost;Port=3306;Database=testcord;User=testcord;Password=testcord;";
        var optionsBuilder = new DbContextOptionsBuilder<TestcordDbContext>();
        var serverVersion = new MySqlServerVersion(new Version(8, 4, 0));

        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new TestcordDbContext(optionsBuilder.Options);
    }
}
