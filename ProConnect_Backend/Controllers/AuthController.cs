using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Application.UseCases.Login.Command;
using ProConnect_Backend.Application.UseCases.Login.Query;
using ProConnect_Backend.Application.UseCases.Users.Query;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginCommandHandler _loginHandler;
    private readonly ProConnect_Backend.Application.UseCases.Users.Command.RegisterCommandHandler _registerHandler;
    private readonly ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQueryHandler _getUserHandler;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        LoginCommandHandler loginHandler,
        ProConnect_Backend.Application.UseCases.Users.Command.RegisterCommandHandler registerHandler,
        ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQueryHandler getUserHandler,
        ILogger<AuthController> logger)
    {
        _loginHandler = loginHandler;
        _registerHandler = registerHandler;
        _getUserHandler = getUserHandler;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ Datos inválidos enviados a login");
                return BadRequest(new
                {
                    success = false,
                    message = "⚠️ Los datos enviados no son válidos. Revisa el formato del correo y la contraseña.",
                    errors = ModelState
                });
            }

            var command = new LoginCommand(dto);
            var result = await _loginHandler.Handle(command);

            if (result == null)
            {
                _logger.LogWarning("🚫 Intento de login fallido para el correo: {Email}", dto.Email);
                return Unauthorized(new
                {
                    success = false,
                    message = "🚫 Correo o contraseña incorrectos. Inténtalo nuevamente."
                });
            }

            _logger.LogInformation("✅ Usuario autenticado correctamente: {Email}", result.Email);

            return Ok(new
            {
                success = true,
                message = "🎉 Inicio de sesión exitoso. ¡Bienvenido/a de nuevo!",
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error inesperado durante el proceso de login");

            return StatusCode(500, new
            {
                success = false,
                message = "💥 Ocurrió un error interno al procesar tu solicitud. Intenta nuevamente más tarde.",
                details = ex.Message
            });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] ProConnect_Backend.Domain.DTOsRequest.AuthDtos.RegisterRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ Datos inválidos enviados a register");
                return BadRequest(new
                {
                    success = false,
                    message = "⚠️ Los datos enviados no son válidos.",
                    errors = ModelState
                });
            }

            var command = new ProConnect_Backend.Application.UseCases.Users.Command.RegisterCommand(dto);
            var result = await _registerHandler.Handle(command);

            _logger.LogInformation("✅ Usuario registrado correctamente: {Email}", result.Email);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error inesperado durante el proceso de register");
            return StatusCode(500, new
            {
                success = false,
                message = "💥 Ocurrió un error interno al procesar tu solicitud.",
                details = ex.Message
            });
        }
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        try
        {
            var query = new ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQuery(id);
            var result = await _getUserHandler.Handle(query);

            if (result == null)
            {
                _logger.LogWarning("❌ Usuario no encontrado: {Id}", id);
                return NotFound(new
                {
                    success = false,
                    message = "Usuario no encontrado"
                });
            }

            _logger.LogInformation("✅ Usuario recuperado: {Id}", id);
            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al recuperar usuario {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al recuperar el usuario"
            });
        }
    }
}