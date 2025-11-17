using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.UpdateProfessionCategory;

public class UpdateProfessionCategoryHandler : IRequestHandler<UpdateProfessionCategoryCommand, ProfessionCategoryResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProfessionCategoryHandler> _logger;

    public UpdateProfessionCategoryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateProfessionCategoryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfessionCategoryResponseDto> Handle(UpdateProfessionCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Actualizando categoría: {CategoryId}", request.CategoryId);

        // Buscar la categoría
        var category = await _unitOfWork.ProfessionCategoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            _logger.LogWarning("❌ Categoría no encontrada: {CategoryId}", request.CategoryId);
            throw new KeyNotFoundException($"No se encontró la categoría con ID {request.CategoryId}");
        }

        // Validar que no exista otra categoría con el mismo nombre
        var existingCategory = await _unitOfWork.ProfessionCategoryRepository.GetByNameAsync(request.Dto.CategoryName);
        if (existingCategory != null && existingCategory.CategoryId != request.CategoryId)
        {
            _logger.LogWarning("⚠️ Ya existe otra categoría con el nombre: {CategoryName}", request.Dto.CategoryName);
            throw new InvalidOperationException($"Ya existe otra categoría con el nombre '{request.Dto.CategoryName}'");
        }

        // Actualizar campos
        category.CategoryName = request.Dto.CategoryName;
        category.Description = request.Dto.Description;

        _unitOfWork.ProfessionCategoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Categoría actualizada: {CategoryId} - {CategoryName}", 
            category.CategoryId, category.CategoryName);

        var response = _mapper.Map<ProfessionCategoryResponseDto>(category);
        response.TotalProfessions = category.Professions?.Count ?? 0;
        
        return response;
    }
}
