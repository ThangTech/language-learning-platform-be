using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            service = "LanguagePlatform.API",
            timestamp = DateTime.UtcNow
        });
    }
}
