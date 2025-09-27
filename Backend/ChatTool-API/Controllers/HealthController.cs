using Microsoft.AspNetCore.Mvc;

namespace ChatTool.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{

    public HealthController()
    {
    }

    // GET
    [HttpGet]
    public IActionResult Status()
    {
        return Ok();
    }
}
