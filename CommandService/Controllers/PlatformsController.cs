using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # CommandService");
        return Ok("Inbound test of from PlatformsController");
    }
}