using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Profession.Queries.GetProfessionsByCategory;

public class GetProfessionsByCategoryHandler : IRequestHandler<GetProfessionsByCategoryQuery, IEnumerable<ProfessionResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProfessionsByCategoryHandler> _logger;

    public GetProfessionsByCategoryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProfessionsByCategoryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProfessionResponseDto>> Handle(GetProfessionsByCategoryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📋 Obteniendo profesiones de la categoría: {CategoryId}", request.CategoryId);

        // Validar que la categoría exista
        var category = await _unitOfWork.ProfessionCategoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            _logger.LogWarning("❌ Categoría no encontrada: {CategoryId}", request.CategoryId);
            throw new KeyNotFoundException($"No se encontró la categoría con ID {request.CategoryId}");
        }

        var professions = await _unitOfWork.ProfessionRepository.GetProfessionsByCategoryAsync(request.CategoryId);
        
        var response = professions.Select(p =>
        {
            var dto = _mapper.Map<ProfessionResponseDto>(p);
            dto.CategoryName = category.CategoryName;
            dto.TotalSpecializations = p.Specializations?.Count ?? 0;
            return dto;
        }).ToList();

        _logger.LogInformation("✅ Se encontraron {Count} profesiones en la categoría {CategoryName}", 
            response.Count, category.CategoryName);
        
        return response;
    }
}
