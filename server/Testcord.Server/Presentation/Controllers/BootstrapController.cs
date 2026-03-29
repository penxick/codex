using Microsoft.AspNetCore.Mvc;

namespace Testcord.Server.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BootstrapController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Product = "Testcord",
            Realtime = new[]
            {
                "/hubs/presence",
                "/hubs/messaging"
            },
            Modules = new[]
            {
                "Auth",
                "Users",
                "Friends",
                "Messaging",
                "Servers",
                "Channels",
                "Voice",
                "Settings",
                "Notifications"
            }
        });
    }
}
