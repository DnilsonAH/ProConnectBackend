using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.UseCases.Users.Query;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly GetUserByIdQueryHandler _getUserHandler;
    private readonly ILogger<UserController> _logger;

    public UserController(GetUserByIdQueryHandler getUserHandler, ILogger<UserController> logger)
    {
        _getUserHandler = getUserHandler;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _getUserHandler.Handle(query);

        if (result == null)
        {
            _logger.LogWarning("Usuario no encontrado: {Id}", id);
            return NotFound(new { message = "Usuario no encontrado" });
        }

        _logger.LogInformation("Usuario recuperado: {Id}", id);
        return Ok(result);
    }
}