using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Profession.Commands.DeleteProfession;

public class DeleteProfessionHandler : IRequestHandler<DeleteProfessionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProfessionHandler> _logger;

    public DeleteProfessionHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteProfessionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProfessionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🗑️ Eliminando profesión: {ProfessionId}", request.ProfessionId);

        var profession = await _unitOfWork.ProfessionRepository.GetByIdAsync(request.ProfessionId);
        if (profession == null)
        {
            _logger.LogWarning("❌ Profesión no encontrada: {ProfessionId}", request.ProfessionId);
            throw new KeyNotFoundException($"No se encontró la profesión con ID {request.ProfessionId}");
        }

        // Validar que no tenga especializaciones asociadas
        // Obtener especializaciones por profession usando el repository de specialization
        var specializations = await _unitOfWork.SpecializationRepository.GetAllAsync();
        var professionSpecializations = specializations.Where(s => s.ProfessionId == request.ProfessionId).ToList();
        
        if (professionSpecializations.Any())
        {
            _logger.LogWarning("⚠️ No se puede eliminar la profesión {ProfessionId} porque tiene {Count} especializaciones asociadas", 
                request.ProfessionId, professionSpecializations.Count);
            throw new InvalidOperationException("No se puede eliminar la profesión porque tiene especializaciones asociadas");
        }

        _unitOfWork.ProfessionRepository.Delete(profession);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Profesión eliminada: {ProfessionId}", request.ProfessionId);
        return true;
    }
}
