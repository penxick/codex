using Microsoft.AspNetCore.Mvc;
using Testcord.Server.Infrastructure.Persistence;

namespace Testcord.Server.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] TestcordDbContext dbContext, CancellationToken cancellationToken)
    {
        try
        {
            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

            return Ok(new
            {
                Status = canConnect ? "Healthy" : "Degraded",
                Database = canConnect ? "Connected" : "Unavailable",
                UtcNow = DateTimeOffset.UtcNow
            });
        }
        catch (Exception exception)
        {
            return Ok(new
            {
                Status = "Degraded",
                Database = "Unavailable",
                Error = exception.Message,
                UtcNow = DateTimeOffset.UtcNow
            });
        }
    }
}
