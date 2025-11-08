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
}