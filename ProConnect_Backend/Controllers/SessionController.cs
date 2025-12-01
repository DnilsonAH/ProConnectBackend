using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.Session.Queries.GetConnectionData;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SessionController> _logger;

    // Inyectamos solo IMediator. El controlador ya no necesita conocer UnitOfWork ni Repositorios.
    public SessionController(
        IMediator mediator,
        ILogger<SessionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
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

            // 2. Crear la Query con los datos necesarios
            var query = new GetSessionConnectionDataQuery(sessionId, currentUserId);

            // 3. Enviar al mediador (él buscará el Handler correspondiente)
            var result = await _mediator.Send(query);

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