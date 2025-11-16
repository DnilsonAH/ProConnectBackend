using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProConnect_Backend.Application.UseCases.Auth.Commands.Login;
using ProConnect_Backend.Application.UseCases.Auth.Commands.Logout;
using ProConnect_Backend.Application.UseCases.Auth.Commands.Register;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IMediator mediator,
        ILogger<AuthController> logger)
    {
        _mediator = mediator;
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
            var result = await _mediator.Send(command);

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

            var command = new RegisterCommand(dto);
            var result = await _mediator.Send(command);

            _logger.LogInformation("✅ Usuario registrado correctamente: {Email}", result.Email);

            return CreatedAtAction(nameof(Register), null, new { success = true, data = result });
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

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            // Obtener el token del header Authorization
            var authHeader = Request.Headers["Authorization"].ToString();
            
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                _logger.LogWarning("⚠️ Intento de logout sin token válido");
                return BadRequest(new
                {
                    success = false,
                    message = "⚠️ Token no proporcionado"
                });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var command = new LogoutCommand(token);
            var result = await _mediator.Send(command);

            if (!result)
            {
                _logger.LogWarning("❌ Error al revocar el token");
                return BadRequest(new
                {
                    success = false,
                    message = "❌ Error al cerrar sesión"
                });
            }

            _logger.LogInformation("✅ Sesión cerrada correctamente");
            return Ok(new
            {
                success = true,
                message = "👋 Sesión cerrada exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error inesperado durante el logout");
            return StatusCode(500, new
            {
                success = false,
                message = "💥 Error interno al cerrar sesión",
                details = ex.Message
            });
        }
    }
}