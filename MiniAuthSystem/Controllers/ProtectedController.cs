using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniAuthSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProtectedController : ControllerBase
{
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var username = User.Identity?.Name;
        var userId = User.FindFirst("sub")?.Value;

        return Ok(new
        {
            Message = "This is a protected endpoint - authentication required",
            UserId = userId,
            Username = username,
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("data")]
    public IActionResult GetData()
    {
        return Ok(new
        {
            Message = "Protected data accessed successfully",
            Data = new[]
            {
                "Secret Item 1",
                "Secret Item 2",
                "Secret Item 3"
            },
            AccessedBy = User.Identity?.Name,
            Timestamp = DateTime.UtcNow
        });
    }
}