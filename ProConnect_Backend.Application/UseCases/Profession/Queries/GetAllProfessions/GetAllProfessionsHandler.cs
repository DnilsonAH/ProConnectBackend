using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Profession.Queries.GetAllProfessions;

public class GetAllProfessionsHandler : IRequestHandler<GetAllProfessionsQuery, IEnumerable<ProfessionResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProfessionsHandler> _logger;

    public GetAllProfessionsHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetAllProfessionsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProfessionResponseDto>> Handle(GetAllProfessionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📋 Obteniendo todas las profesiones");

        var professions = await _unitOfWork.ProfessionRepository.GetAllAsync();
        
        var response = professions.Select(p =>
        {
            var dto = _mapper.Map<ProfessionResponseDto>(p);
            dto.CategoryName = p.Category?.CategoryName ?? "Sin categoría";
            dto.TotalSpecializations = p.Specializations?.Count ?? 0;
            return dto;
        }).ToList();

        _logger.LogInformation("✅ Se encontraron {Count} profesiones", response.Count);
        return response;
    }
}
