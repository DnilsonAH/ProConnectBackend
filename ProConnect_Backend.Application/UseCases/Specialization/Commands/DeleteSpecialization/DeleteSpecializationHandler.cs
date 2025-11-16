using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialization.Commands.DeleteSpecialization;

public class DeleteSpecializationHandler : IRequestHandler<DeleteSpecializationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpecializationHandler> _logger;

    public DeleteSpecializationHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteSpecializationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🗑️ Eliminando especialización: {SpecializationId}", request.SpecializationId);

        var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(request.SpecializationId);
        if (specialization == null)
        {
            _logger.LogWarning("❌ Especialización no encontrada: {SpecializationId}", request.SpecializationId);
            throw new KeyNotFoundException($"No se encontró la especialización con ID {request.SpecializationId}");
        }

        // Validar que no tenga perfiles asociados
        var profileSpecializations = await _unitOfWork.ProfileSpecializationRepository.GetBySpecializationIdAsync(request.SpecializationId);
        if (profileSpecializations.Any())
        {
            _logger.LogWarning("⚠️ No se puede eliminar la especialización {SpecializationId} porque tiene {Count} perfiles asociados", 
                request.SpecializationId, profileSpecializations.Count());
            throw new InvalidOperationException("No se puede eliminar la especialización porque tiene perfiles profesionales asociados");
        }

        _unitOfWork.SpecializationRepository.Delete(specialization);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Especialización eliminada: {SpecializationId}", request.SpecializationId);
        return true;
    }
}
