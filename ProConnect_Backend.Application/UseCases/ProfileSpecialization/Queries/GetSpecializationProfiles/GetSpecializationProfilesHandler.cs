using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Queries.GetSpecializationProfiles;

public class GetSpecializationProfilesHandler : IRequestHandler<GetSpecializationProfilesQuery, IEnumerable<ProfileSpecializationResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSpecializationProfilesHandler> _logger;

    public GetSpecializationProfilesHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetSpecializationProfilesHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProfileSpecializationResponseDto>> Handle(GetSpecializationProfilesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📋 Obteniendo perfiles con la especialización: {SpecializationId}", request.SpecializationId);

        // Validar que la especialización exista
        var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(request.SpecializationId);
        if (specialization == null)
        {
            _logger.LogWarning("❌ Especialización no encontrada: {SpecializationId}", request.SpecializationId);
            throw new KeyNotFoundException($"No se encontró la especialización con ID {request.SpecializationId}");
        }

        var profileSpecializations = await _unitOfWork.ProfileSpecializationRepository.GetBySpecializationIdAsync(request.SpecializationId);
        
        var response = profileSpecializations.Select(ps =>
        {
            var dto = _mapper.Map<ProfileSpecializationResponseDto>(ps);
            dto.SpecializationName = specialization.SpecializationName;
            dto.ProfessionName = specialization.Profession?.ProfessionName ?? "Sin profesión";
            return dto;
        }).ToList();

        _logger.LogInformation("✅ Se encontraron {Count} perfiles con la especialización", response.Count);
        return response;
    }
}
