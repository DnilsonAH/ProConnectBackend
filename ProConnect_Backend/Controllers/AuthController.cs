using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Application.UseCases.Login.Command;
using ProConnect_Backend.Application.UseCases.Login.Query;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginCommandHandler _loginHandler;
    private readonly ILogger<AuthController> _logger;

    public AuthController(LoginCommandHandler loginHandler, ILogger<AuthController> logger)
    {
        _loginHandler = loginHandler;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new LoginCommand(dto);
        var result = await _loginHandler.Handle(command);

        if (result == null)
            return Unauthorized(new { message = "Correo o contraseña incorrectos." });

        _logger.LogInformation("Usuario autenticado: {Email}", result.Email);

        return Ok(result);
    }
}