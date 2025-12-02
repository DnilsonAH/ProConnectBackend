using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.Session.Commands;
using ProConnect_Backend.Application.UseCases.Session.Queries;
using ProConnect_Backend.Application.UseCases.Session.Queries.GetConnectionData;
using ProConnect_Backend.Domain.DTOsRequest.SessionDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SessionController> _logger;


    public SessionController(
        IMediator mediator,
        ILogger<SessionController> logger
    )
    {
        _mediator = mediator;
        _logger = logger;
    }
[HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllSessionsQuery();
            var result = await _mediator.Send(query);
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las sesiones.");
            return StatusCode(500, new { success = false, message = "Error interno al obtener las sesiones." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        try
        {
            var query = new GetSessionByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null) 
                return NotFound(new { success = false, message = "Sesión no encontrada." });

            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la sesión {Id}", id);
            return StatusCode(500, new { success = false, message = "Error interno al obtener la sesión." });
        }
    }

    [HttpGet("my-sessions/future")]
    public async Task<IActionResult> GetMyFutureSessions()
    {
        try
        {
            var query = new GetMyFutureSessionsQuery();
            var result = await _mediator.Send(query);
            return Ok(new { success = true, data = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sesiones futuras.");
            return StatusCode(500, new { success = false, message = "Error interno al obtener sesiones futuras." });
        }
    }

    [HttpGet("my-sessions/previous")]
    public async Task<IActionResult> GetMyPreviousSessions()
    {
        try
        {
            var query = new GetMyPreviousSessionsQuery();
            var result = await _mediator.Send(query);
            return Ok(new { success = true, data = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sesiones pasadas.");
            return StatusCode(500, new { success = false, message = "Error interno al obtener sesiones pasadas." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSessionDto model)
    {
        try
        {
            var command = new CreateSessionCommand { Model = model };
            var result = await _mediator.Send(command);
            
            // Retornamos 201 Created con la estructura estándar
            return CreatedAtAction(nameof(GetById), new { id = result.SessionId }, new { success = true, message = "Sesión creada exitosamente.", data = result });
        }
        catch (InvalidOperationException ex) // Errores de lógica de negocio (ej: horario ocupado)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (ArgumentException ex) // Errores de validación de datos
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error crítico al crear la sesión.");
            return StatusCode(500, new { success = false, message = "Error interno al procesar la creación de la sesión." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, UpdateSessionDto model)
    {
        try
        {
            var command = new UpdateSessionCommand { Id = id, Model = model };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new { success = false, message = "Sesión no encontrada para actualizar." });

            return Ok(new { success = true, message = "Sesión actualizada correctamente.", data = result });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la sesión {Id}", id);
            return StatusCode(500, new { success = false, message = "Error interno al actualizar la sesión." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        try
        {
            var command = new DeleteSessionCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { success = false, message = "Sesión no encontrada para eliminar." });

            // 204 No Content es estándar para DELETE, pero no permite body. 
            // Si prefieres devolver JSON, cambia a Ok(new { success = true ... })
            return NoContent(); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la sesión {Id}", id);
            return StatusCode(500, new { success = false, message = "Error interno al eliminar la sesión." });
        }
    }

    /// <summary>
    /// Obtiene los datos de conexión (Token seguro) para una videoconsulta.
    /// Verifica que la sesión exista, que el usuario sea participante y que sea la hora correcta.
    /// </summary>
    /// <param name="sessionId">ID de la sesión</param>
    [HttpGet("{sessionId}/connection-data")]
    public async Task<IActionResult> GetConnectionData(uint sessionId)
    {
        try
        {
            // 1. Obtener ID del usuario autenticado desde el Token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !uint.TryParse(userIdClaim, out uint currentUserId))
            {
                return Unauthorized(new { success = false, message = "Usuario no identificado en el token." });
            }

            // 2. Crear el Query con los parámetros necesarios
            // 3. Enviar al mediador (él buscará el Handler correspondiente)
            var result = await _mediator.Send(new GetSessionConnectionDataQuery(sessionId, currentUserId));

            // 4. Retornar respuesta exitosa
            return Ok(new
            {
                success = true,
                data = result
            });
        }
        // 5. Manejo de Excepciones (Mapeo de errores de lógica a códigos HTTP)
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // InvalidOperationException lo usamos para validaciones de negocio (ej: hora incorrecta)
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error interno al generar token de video para sesión {SessionId}", sessionId);
            return StatusCode(500, new { success = false, message = "Error interno al procesar la solicitud." });
        }
    }
}
