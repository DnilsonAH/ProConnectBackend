using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.DTOs.AuthDtos;
using ProConnect_Backend.Application.Ports;

namespace ProConnect_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Registra un nuevo usuario con el rol "User" por defecto
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error en registro: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado en registro");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Inicia sesión con credenciales de usuario
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Intento de login fallido para: {Email}", loginDto.Email);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado en login");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Cambia el rol de un usuario. Requiere autenticación.
    /// </summary>
    [HttpPut("change-role")]
    [Authorize] // Cualquier usuario autenticado puede cambiar roles (ajusta según tus necesidades)
    public async Task<ActionResult> ChangeRole([FromBody] ChangeRoleRequestDto changeRoleDto)
    {
        try
        {
            var result = await _authService.ChangeUserRoleAsync(changeRoleDto.UserId, changeRoleDto.RoleName);
            
            if (result)
            {
                return Ok(new { message = $"Rol '{changeRoleDto.RoleName}' asignado correctamente al usuario" });
            }

            return BadRequest(new { message = "No se pudo cambiar el rol" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error al cambiar rol: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al cambiar rol");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }
}
