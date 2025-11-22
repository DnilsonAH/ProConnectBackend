using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.WeeklyAvailabilityDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Queries.GetWeeklyAvailabilityByProfessional;

public class GetWeeklyAvailabilityByProfessionalHandler : IRequestHandler<GetWeeklyAvailabilityByProfessionalQuery, IEnumerable<WeeklyAvailabilityResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWeeklyAvailabilityByProfessionalHandler> _logger;

    public GetWeeklyAvailabilityByProfessionalHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetWeeklyAvailabilityByProfessionalHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<WeeklyAvailabilityResponseDto>> Handle(GetWeeklyAvailabilityByProfessionalQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting WeeklyAvailability for Professional ID: {ProfessionalId}", request.ProfessionalId);

        var availabilities = await _unitOfWork.WeeklyAvailabilityRepository.GetAvailabilityByProfessionalIdAsync(request.ProfessionalId);

        return _mapper.Map<IEnumerable<WeeklyAvailabilityResponseDto>>(availabilities);
    }
}
