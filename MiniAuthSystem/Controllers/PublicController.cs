using Microsoft.AspNetCore.Mvc;

namespace MiniAuthSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicController : ControllerBase
{
    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            Message = "This is a public endpoint - no authentication required",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        });
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}