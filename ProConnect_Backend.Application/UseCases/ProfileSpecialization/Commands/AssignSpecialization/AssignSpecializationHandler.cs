using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Commands.AssignSpecialization;

public class AssignSpecializationHandler : IRequestHandler<AssignSpecializationCommand, ProfileSpecializationResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AssignSpecializationHandler> _logger;

    public AssignSpecializationHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AssignSpecializationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfileSpecializationResponseDto> Handle(AssignSpecializationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Asignando especialización {SpecializationId} al perfil {ProfileId}", 
            request.Dto.SpecializationId, request.Dto.ProfileId);

        // Validar que el perfil profesional exista
        var profile = await _unitOfWork.ProfessionalProfileRepository.GetByIdAsync(request.Dto.ProfileId);
        if (profile == null)
        {
            _logger.LogWarning("❌ Perfil profesional no encontrado: {ProfileId}", request.Dto.ProfileId);
            throw new KeyNotFoundException($"No se encontró el perfil profesional con ID {request.Dto.ProfileId}");
        }

        // Validar que la especialización exista
        var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(request.Dto.SpecializationId);
        if (specialization == null)
        {
            _logger.LogWarning("❌ Especialización no encontrada: {SpecializationId}", request.Dto.SpecializationId);
            throw new KeyNotFoundException($"No se encontró la especialización con ID {request.Dto.SpecializationId}");
        }

        // Validar que no exista ya la asignación
        var exists = await _unitOfWork.ProfileSpecializationRepository.ExistsAsync(request.Dto.ProfileId, request.Dto.SpecializationId);
        if (exists)
        {
            _logger.LogWarning("⚠️ La especialización ya está asignada al perfil");
            throw new InvalidOperationException("La especialización ya está asignada a este perfil profesional");
        }

        // Crear la asignación
        var profileSpecialization = new Domain.Entities.ProfileSpecialization
        {
            ProfileId = request.Dto.ProfileId,
            SpecializationId = request.Dto.SpecializationId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ProfileSpecializationRepository.AddAsync(profileSpecialization);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Especialización asignada exitosamente: {ProfileSpecializationId}", 
            profileSpecialization.ProfileSpecializationId);

        var response = _mapper.Map<ProfileSpecializationResponseDto>(profileSpecialization);
        response.SpecializationName = specialization.SpecializationName;
        response.ProfessionName = specialization.Profession?.ProfessionName ?? "Sin profesión";
        
        return response;
    }
}
