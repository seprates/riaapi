using Microsoft.AspNetCore.Mvc;

namespace RIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    /// <summary>
    /// Ensures the API is up and running.
    /// </summary>
    [HttpGet]
    public ActionResult Get()
    {
        return Ok(new
        {
            Status = "Alive",
            DateTimeUtc = DateTime.UtcNow
        });
    }
}