using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Queries.GetProfileSpecializations;

public class GetProfileSpecializationsHandler : IRequestHandler<GetProfileSpecializationsQuery, IEnumerable<ProfileSpecializationResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProfileSpecializationsHandler> _logger;

    public GetProfileSpecializationsHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProfileSpecializationsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProfileSpecializationResponseDto>> Handle(GetProfileSpecializationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📋 Obteniendo especializaciones del perfil: {ProfileId}", request.ProfileId);

        // Validar que el perfil exista
        var profile = await _unitOfWork.ProfessionalProfileRepository.GetByIdAsync(request.ProfileId);
        if (profile == null)
        {
            _logger.LogWarning("❌ Perfil profesional no encontrado: {ProfileId}", request.ProfileId);
            throw new KeyNotFoundException($"No se encontró el perfil profesional con ID {request.ProfileId}");
        }

        var profileSpecializations = await _unitOfWork.ProfileSpecializationRepository.GetByProfileIdAsync(request.ProfileId);
        
        var response = profileSpecializations.Select(ps =>
        {
            var dto = _mapper.Map<ProfileSpecializationResponseDto>(ps);
            dto.SpecializationName = ps.Specialization?.SpecializationName ?? "Sin nombre";
            dto.ProfessionName = ps.Specialization?.Profession?.ProfessionName ?? "Sin profesión";
            return dto;
        }).ToList();

        _logger.LogInformation("✅ Se encontraron {Count} especializaciones para el perfil", response.Count);
        return response;
    }
}
