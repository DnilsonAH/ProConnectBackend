using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialization.Queries.GetAllSpecializations;

public class GetAllSpecializationsHandler : IRequestHandler<GetAllSpecializationsQuery, IEnumerable<SpecializationResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSpecializationsHandler> _logger;

    public GetAllSpecializationsHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetAllSpecializationsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SpecializationResponseDto>> Handle(GetAllSpecializationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📋 Obteniendo todas las especializaciones");

        var specializations = await _unitOfWork.SpecializationRepository.GetAllAsync();
        
        var response = specializations.Select(s =>
        {
            var dto = _mapper.Map<SpecializationResponseDto>(s);
            dto.ProfessionName = s.Profession?.ProfessionName ?? "Sin profesión";
            dto.TotalProfiles = s.ProfileSpecializations?.Count ?? 0;
            return dto;
        }).ToList();

        _logger.LogInformation("✅ Se encontraron {Count} especializaciones", response.Count);
        return response;
    }
}
