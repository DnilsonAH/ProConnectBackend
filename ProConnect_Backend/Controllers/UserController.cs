using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProConnect_Backend.Application.UseCases.Users.Queries.GetUserInfo;
using ProConnect_Backend.Application.UseCases.Users.Commands.UpdateUser;
using ProConnect_Backend.Domain.DTOsRequest.UserDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IMediator mediator,
        ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la información del usuario autenticado actual
    /// </summary>
    /// <returns>Información del usuario logueado</returns>
    [Authorize]
    [HttpGet("me")]
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

            var query = new GetUserByIdQuery(userId);
            var result = await _mediator.Send(query);

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

    /// <summary>
    /// Obtiene la información de un usuario específico por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Información del usuario solicitado</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);

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

    /// <summary>
    /// Actualiza la información de un usuario (requiere autenticación)
    /// </summary>
    /// <param name="id">ID del usuario a actualizar</param>
    /// <param name="dto">Datos del usuario a actualizar</param>
    /// <returns>Usuario actualizado</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(uint id, [FromBody] UpdateUserRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ Datos inválidos enviados a UpdateUser");
                return BadRequest(new
                {
                    success = false,
                    message = "⚠️ Los datos enviados no son válidos.",
                    errors = ModelState
                });
            }

            // Verificar que el usuario autenticado solo pueda actualizar su propia información
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !uint.TryParse(userIdClaim, out uint authenticatedUserId))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Token inválido"
                });
            }

            // Solo permitir que un usuario actualice su propia información (a menos que sea admin)
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (authenticatedUserId != id && userRole != "Admin")
            {
                _logger.LogWarning("🚫 Usuario {AuthUserId} intentó actualizar usuario {TargetUserId}", authenticatedUserId, id);
                return Forbid();
            }

            var command = new UpdateUserCommand(id, dto);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("❌ Usuario no encontrado: {Id}", id);
                return NotFound(new
                {
                    success = false,
                    message = "Usuario no encontrado"
                });
            }

            _logger.LogInformation("✅ Usuario actualizado: {Id}", id);
            return Ok(new
            {
                success = true,
                message = "Usuario actualizado exitosamente",
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al actualizar usuario {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al actualizar el usuario"
            });
        }
    }
}
