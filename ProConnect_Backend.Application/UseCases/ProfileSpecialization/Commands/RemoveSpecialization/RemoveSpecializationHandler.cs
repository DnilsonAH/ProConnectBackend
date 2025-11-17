using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Commands.RemoveSpecialization;

public class RemoveSpecializationHandler : IRequestHandler<RemoveSpecializationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveSpecializationHandler> _logger;

    public RemoveSpecializationHandler(
        IUnitOfWork unitOfWork,
        ILogger<RemoveSpecializationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(RemoveSpecializationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🗑️ Removiendo especialización {SpecializationId} del perfil {ProfileId}", 
            request.SpecializationId, request.ProfileId);

        // Buscar la asignación
        var profileSpecialization = await _unitOfWork.ProfileSpecializationRepository
            .GetByProfileAndSpecializationAsync(request.ProfileId, request.SpecializationId);

        if (profileSpecialization == null)
        {
            _logger.LogWarning("❌ No se encontró la asignación de la especialización al perfil");
            throw new KeyNotFoundException("No se encontró la asignación de esta especialización al perfil profesional");
        }

        _unitOfWork.ProfileSpecializationRepository.Delete(profileSpecialization);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Especialización removida exitosamente del perfil");
        return true;
    }
}
