using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IServices;

namespace ProConnect_Backend.Application.UseCases.Session.Queries.GetConnectionData;

public class GetSessionConnectionDataHandler : IRequestHandler<GetSessionConnectionDataQuery, ConnectionDataResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoCallService _videoCallService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetSessionConnectionDataHandler> _logger;
    private readonly ITimeZoneConverter _timeZoneConverter;

    public GetSessionConnectionDataHandler(
        IUnitOfWork unitOfWork,
        IVideoCallService videoCallService,
        IConfiguration configuration,
        ITimeZoneConverter timeZoneConverter,
        ILogger<GetSessionConnectionDataHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _videoCallService = videoCallService;
        _configuration = configuration;
        _logger = logger;
        _timeZoneConverter = timeZoneConverter;
    }

    public async Task<ConnectionDataResponseDto> Handle(GetSessionConnectionDataQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Procesando solicitud de video para Sesión {SessionId} por Usuario {UserId}", request.SessionId, request.UserId);

        // 1. Obtener la sesión con sus detalles (incluye Cliente y Profesional)
        // Usamos el repositorio a través del UnitOfWork (Patrón Repository)
        var session = await _unitOfWork.SessionRepository.GetSessionWithDetailsAsync(request.SessionId);

        if (session == null)
        {
            _logger.LogWarning("Sesión {SessionId} no encontrada", request.SessionId);
            throw new KeyNotFoundException("La sesión solicitada no existe.");
        }

        // 2. SEGURIDAD: Verificar que el usuario pertenezca a esta cita.
        // Solo el Cliente o el Profesional asociados pueden acceder.
        if (session.ClientId != request.UserId && session.ProfessionalId != request.UserId)
        {
            _logger.LogWarning("Acceso denegado: Usuario {UserId} no es participante de la sesión {SessionId}", request.UserId, request.SessionId);
            throw new UnauthorizedAccessException("Acceso denegado. No eres participante de esta consulta.");
        }
        
        // 3. LÓGICA DE NEGOCIO: Verificar rango de tiempo.
        // Se asume que las fechas en BD están en UTC o el servidor maneja la misma zona horaria.
        // Llamas al método de instancia a través de la interfaz
        DateTime now = _timeZoneConverter.GetLocalTimeInColombiaPeru();
  
        
        // Damos un margen de tolerancia (ej. 10 minutos antes) para que puedan entrar a probar conexión.
        // Si quieres ser estricto, quita el .AddMinutes(-10).
        if (now < session.StartDate.AddMinutes(-10)) 
        {
            throw new InvalidOperationException($"La sesión aún no ha iniciado. Está programada para: {session.StartDate:g}");
        }

        if (now > session.EndDate)
        {
            throw new InvalidOperationException("La sesión ya ha finalizado.");
        }

        // 4. Preparar datos
        string roomId = session.SessionId.ToString();
        string userIdString = request.UserId.ToString();

        // Determinar el nombre a mostrar en la videollamada
        string userName = (session.ClientId == request.UserId)
            ? $"{session.Client.FirstName} {session.Client.FirstSurname}"
            : $"{session.Professional.FirstName} {session.Professional.FirstSurname}";

        // 5. Generar Token usando el servicio de infraestructura (ZegoCloud)
        string token = _videoCallService.GenerateCallToken(userIdString, roomId);

        // Obtener AppId de la configuración
        if (!uint.TryParse(_configuration["ZegoCloud:AppId"], out uint appId))
        {
            _logger.LogError("ZegoCloud AppId no configurado correctamente.");
            throw new Exception("Error de configuración del servidor (AppId).");
        }

        _logger.LogInformation("Token generado exitosamente para Sesión {SessionId}", request.SessionId);

        // 6. Retornar DTO
        return new ConnectionDataResponseDto
        {
            AppId = appId,
            AppSign = "", // Por seguridad, vacío si usamos Token Auth
            Token = token,
            RoomId = roomId,
            UserId = userIdString,
            UserName = userName
        };
    }
}