using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.CreateProfessionCategory;

public class CreateProfessionCategoryHandler : IRequestHandler<CreateProfessionCategoryCommand, ProfessionCategoryResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProfessionCategoryHandler> _logger;

    public CreateProfessionCategoryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateProfessionCategoryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfessionCategoryResponseDto> Handle(CreateProfessionCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Creando nueva categoría de profesión: {CategoryName}", request.Dto.CategoryName);

        // Validar que no exista una categoría con el mismo nombre
        var existingCategory = await _unitOfWork.ProfessionCategoryRepository.GetByNameAsync(request.Dto.CategoryName);
        if (existingCategory != null)
        {
            _logger.LogWarning("⚠️ Ya existe una categoría con el nombre: {CategoryName}", request.Dto.CategoryName);
            throw new InvalidOperationException($"Ya existe una categoría con el nombre '{request.Dto.CategoryName}'");
        }

        // Crear la entidad
        var category = _mapper.Map<Domain.Entities.ProfessionCategory>(request.Dto);
        
        await _unitOfWork.ProfessionCategoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Categoría creada exitosamente: {CategoryId} - {CategoryName}", 
            category.CategoryId, category.CategoryName);

        var response = _mapper.Map<ProfessionCategoryResponseDto>(category);
        response.TotalProfessions = 0; // Nueva categoría no tiene profesiones
        
        return response;
    }
}
