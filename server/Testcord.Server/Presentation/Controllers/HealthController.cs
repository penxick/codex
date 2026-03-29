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

            if (!canConnect)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    Status = "Unavailable",
                    Database = "Unavailable",
                    UtcNow = DateTimeOffset.UtcNow
                });
            }

            return Ok(new
            {
                Status = "Healthy",
                Database = "Connected",
                UtcNow = DateTimeOffset.UtcNow
            });
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                Status = "Unavailable",
                Database = "Unavailable",
                Error = exception.Message,
                UtcNow = DateTimeOffset.UtcNow
            });
        }
    }
}
