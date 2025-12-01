using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IServices;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // 🔒 IMPORTANTE: Solo usuarios autenticados pueden usar esto
public class SessionController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoCallService _videoCallService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SessionController> _logger;

    public SessionController(
        IUnitOfWork unitOfWork,
        IVideoCallService videoCallService,
        IConfiguration configuration,
        ILogger<SessionController> logger)
    {
        _unitOfWork = unitOfWork;
        _videoCallService = videoCallService;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene los datos de conexión (Token seguro) para una videoconsulta específica.
    /// Valida que el usuario sea parte de la sesión (Cliente o Profesional).
    /// </summary>
    /// <param name="sessionId">ID de la cita/sesión</param>
    [HttpGet("{sessionId}/connection-data")]
    public async Task<IActionResult> GetConnectionData(uint sessionId)
    {
        try
        {
            // 1. Identificar quién está llamando (extraer ID del JWT)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value;
                           
            if (string.IsNullOrEmpty(userIdClaim) || !uint.TryParse(userIdClaim, out uint currentUserId))
            {
                return Unauthorized(new { success = false, message = "Usuario no identificado en el token." });
            }

            // 2. Buscar la sesión en la base de datos
            var session = await _unitOfWork.SessionRepository.GetSessionWithDetailsAsync(sessionId);
            
            if (session == null)
            {
                return NotFound(new { success = false, message = "La sesión solicitada no existe." });
            }

            // 3. 🛡️ SEGURIDAD: Verificar que el usuario pertenezca a esta cita
            // Solo el Cliente (Paciente) o el Profesional (Doctor/Abogado) pueden obtener el token.
            if (session.ClientId != currentUserId && session.ProfessionalId != currentUserId)
            {
                _logger.LogWarning("ALERTA DE SEGURIDAD: Usuario {UserId} intentó acceder a sesión ajena {SessionId}", currentUserId, sessionId);
                return StatusCode(403, new { success = false, message = "Acceso denegado. No eres participante de esta consulta." });
            }

            // 4. Preparar datos para el Token
            string roomId = session.SessionId.ToString(); // El ID de la sala es el ID de la sesión
            string userIdString = currentUserId.ToString();
            
            // Nombre para mostrar en la interfaz de video
            string userName = (session.ClientId == currentUserId)
                ? $"{session.Client.FirstName} {session.Client.FirstSurname}"
                : $"{session.Professional.FirstName} {session.Professional.FirstSurname}";

            // 5. Generar el Token usando nuestro servicio de infraestructura
            string token = _videoCallService.GenerateCallToken(userIdString, roomId);
            
            // Obtener AppID para que el frontend sepa a qué app conectarse
            if (!uint.TryParse(_configuration["ZegoCloud:AppId"], out uint appId))
            {
                 // Si falla, retornamos 500 porque es un error de configuración del servidor
                 _logger.LogError("ZegoCloud AppId no configurado correctamente en el servidor.");
                 return StatusCode(500, new { success = false, message = "Error de configuración del servidor." });
            }

            _logger.LogInformation("✅ Token de video generado exitosamente para Sesión {SessionId} por Usuario {UserId}", sessionId, currentUserId);

            // 6. Retornar todo lo que Flutter necesita
            return Ok(new
            {
                success = true,
                data = new
                {
                    appId = appId,
                    appSign = "", // Dejamos vacío por seguridad, usamos Token Authentication
                    token = token,
                    roomId = roomId,
                    userId = userIdString,
                    userName = userName
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción al generar token ZegoCloud para sesión {SessionId}", sessionId);
            return StatusCode(500, new { success = false, message = "Error interno al procesar la solicitud de video." });
        }
    }
}