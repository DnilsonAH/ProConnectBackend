using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Commands.DeleteProfessionalProfile;

public class DeleteProfessionalProfileHandler : IRequestHandler<DeleteProfessionalProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProfessionalProfileHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteProfessionalProfileHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteProfessionalProfileHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(DeleteProfessionalProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando eliminación de perfil profesional {ProfileId}", request.ProfileId);

        var entity = await _unitOfWork.ProfessionalProfileRepository.GetByIdAsync(request.ProfileId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Perfil profesional con ID {request.ProfileId} no encontrado");
        }

        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        var userRoleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null || !uint.TryParse(userIdClaim.Value, out uint userId))
        {
            throw new UnauthorizedAccessException("Usuario no autenticado");
        }

        // Validar permisos: Admin o dueño
        if (userRoleClaim?.Value != "Admin" && entity.UserId != userId)
        {
            throw new UnauthorizedAccessException("No tiene permisos para eliminar este perfil");
        }

        _unitOfWork.ProfessionalProfileRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Perfil profesional {ProfileId} eliminado exitosamente", request.ProfileId);
    }
}
