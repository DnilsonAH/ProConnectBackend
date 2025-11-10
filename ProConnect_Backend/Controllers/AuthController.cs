using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Application.UseCases.Login.Command;
using ProConnect_Backend.Application.UseCases.Login.Query;
using ProConnect_Backend.Application.UseCases.Users.Query;
using ProConnect_Backend.Application.UseCases.Logout.Command;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginCommandHandler _loginHandler;
    private readonly ProConnect_Backend.Application.UseCases.Users.Command.RegisterCommandHandler _registerHandler;
    private readonly ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQueryHandler _getUserHandler;
    private readonly LogoutCommandHandler _logoutHandler;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        LoginCommandHandler loginHandler,
        ProConnect_Backend.Application.UseCases.Users.Command.RegisterCommandHandler registerHandler,
        ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQueryHandler getUserHandler,
        LogoutCommandHandler logoutHandler,
        ILogger<AuthController> logger)
    {
        _loginHandler = loginHandler;
        _registerHandler = registerHandler;
        _getUserHandler = getUserHandler;
        _logoutHandler = logoutHandler;
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

            return CreatedAtAction(nameof(GetCurrentUser), null, new { success = true, data = result });
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
    [HttpGet("user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            // Obtener el ID del usuario desde el token JWT
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                _logger.LogWarning("⚠️ No se pudo obtener el ID del usuario del token");
                return Unauthorized(new
                {
                    success = false,
                    message = "Token inválido: no contiene información del usuario"
                });
            }

            if (!uint.TryParse(userIdClaim, out uint userId))
            {
                _logger.LogWarning("⚠️ ID de usuario inválido en el token: {UserIdClaim}", userIdClaim);
                return BadRequest(new
                {
                    success = false,
                    message = "Token inválido: ID de usuario no válido"
                });
            }

            var query = new ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQuery(userId);
            var result = await _getUserHandler.Handle(query);

            if (result == null)
            {
                _logger.LogWarning("❌ Usuario no encontrado: {Id}", userId);
                return NotFound(new
                {
                    success = false,
                    message = "Usuario no encontrado"
                });
            }

            _logger.LogInformation("✅ Usuario recuperado desde JWT: {Id} - {Email}", userId, result.Email);
            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al recuperar usuario actual");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al recuperar el usuario"
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
            var result = await _logoutHandler.Handle(command);

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