using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Queries.GetProfessionCategoryById;

public class GetProfessionCategoryByIdHandler : IRequestHandler<GetProfessionCategoryByIdQuery, ProfessionCategoryResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProfessionCategoryByIdHandler> _logger;

    public GetProfessionCategoryByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProfessionCategoryByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfessionCategoryResponseDto?> Handle(GetProfessionCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔍 Buscando categoría: {CategoryId}", request.CategoryId);

        var category = await _unitOfWork.ProfessionCategoryRepository.GetByIdAsync(request.CategoryId);
        
        if (category == null)
        {
            _logger.LogWarning("❌ Categoría no encontrada: {CategoryId}", request.CategoryId);
            return null;
        }

        var response = _mapper.Map<ProfessionCategoryResponseDto>(category);
        response.TotalProfessions = category.Professions?.Count ?? 0;

        _logger.LogInformation("✅ Categoría encontrada: {CategoryId} - {CategoryName}", 
            category.CategoryId, category.CategoryName);
        
        return response;
    }
}
