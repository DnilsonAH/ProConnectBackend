using Microsoft.AspNetCore.Mvc;

namespace ProConnect_Backend.Controllers.Endpoints_Documentations;

[ApiController]
[Route("api/[controller]")]
public class DisponibilityController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult CheckDisponibility()
    {
        return Ok("API Disponible");
    }
}