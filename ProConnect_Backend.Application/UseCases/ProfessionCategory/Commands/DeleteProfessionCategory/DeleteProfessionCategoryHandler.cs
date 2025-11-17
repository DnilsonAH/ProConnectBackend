using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.DeleteProfessionCategory;

public class DeleteProfessionCategoryHandler : IRequestHandler<DeleteProfessionCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProfessionCategoryHandler> _logger;

    public DeleteProfessionCategoryHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteProfessionCategoryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProfessionCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🗑️ Eliminando categoría: {CategoryId}", request.CategoryId);

        var category = await _unitOfWork.ProfessionCategoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            _logger.LogWarning("❌ Categoría no encontrada: {CategoryId}", request.CategoryId);
            throw new KeyNotFoundException($"No se encontró la categoría con ID {request.CategoryId}");
        }

        // Validar que no tenga profesiones asociadas
        var professions = await _unitOfWork.ProfessionRepository.GetProfessionsByCategoryAsync(request.CategoryId);
        if (professions.Any())
        {
            _logger.LogWarning("⚠️ No se puede eliminar la categoría {CategoryId} porque tiene {Count} profesiones asociadas", 
                request.CategoryId, professions.Count());
            throw new InvalidOperationException("No se puede eliminar la categoría porque tiene profesiones asociadas");
        }

        _unitOfWork.ProfessionCategoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Categoría eliminada: {CategoryId}", request.CategoryId);
        return true;
    }
}
