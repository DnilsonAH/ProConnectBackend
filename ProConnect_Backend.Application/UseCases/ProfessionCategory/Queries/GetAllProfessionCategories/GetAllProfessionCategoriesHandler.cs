using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Queries.GetAllProfessionCategories;

public class GetAllProfessionCategoriesHandler : IRequestHandler<GetAllProfessionCategoriesQuery, IEnumerable<ProfessionCategoryResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProfessionCategoriesHandler> _logger;

    public GetAllProfessionCategoriesHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetAllProfessionCategoriesHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProfessionCategoryResponseDto>> Handle(GetAllProfessionCategoriesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📋 Obteniendo todas las categorías de profesiones");

        var categories = await _unitOfWork.ProfessionCategoryRepository.GetCategoriesWithProfessionsAsync();
        
        var response = categories.Select(c =>
        {
            var dto = _mapper.Map<ProfessionCategoryResponseDto>(c);
            dto.TotalProfessions = c.Professions?.Count ?? 0;
            return dto;
        }).ToList();

        _logger.LogInformation("✅ Se encontraron {Count} categorías", response.Count);
        return response;
    }
}
