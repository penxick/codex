using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Testcord.Server.Infrastructure.Persistence;

public sealed class TestcordDbContextFactory : IDesignTimeDbContextFactory<TestcordDbContext>
{
    public TestcordDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("TESTCORD_ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=testcord;Username=postgres;Password=PUT_PASSWORD_HERE";
        var optionsBuilder = new DbContextOptionsBuilder<TestcordDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new TestcordDbContext(optionsBuilder.Options);
    }
}
