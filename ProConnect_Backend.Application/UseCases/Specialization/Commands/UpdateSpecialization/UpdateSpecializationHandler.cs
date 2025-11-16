using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialization.Commands.UpdateSpecialization;

public class UpdateSpecializationHandler : IRequestHandler<UpdateSpecializationCommand, SpecializationResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSpecializationHandler> _logger;

    public UpdateSpecializationHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateSpecializationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SpecializationResponseDto> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Actualizando especialización: {SpecializationId}", request.SpecializationId);

        // Buscar la especialización
        var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(request.SpecializationId);
        if (specialization == null)
        {
            _logger.LogWarning("❌ Especialización no encontrada: {SpecializationId}", request.SpecializationId);
            throw new KeyNotFoundException($"No se encontró la especialización con ID {request.SpecializationId}");
        }

        // Validar que la profesión exista
        var profession = await _unitOfWork.ProfessionRepository.GetByIdAsync(request.Dto.ProfessionId);
        if (profession == null)
        {
            _logger.LogWarning("❌ Profesión no encontrada: {ProfessionId}", request.Dto.ProfessionId);
            throw new KeyNotFoundException($"No se encontró la profesión con ID {request.Dto.ProfessionId}");
        }

        // Validar que no exista otra especialización con el mismo nombre
        var existingSpecialization = await _unitOfWork.SpecializationRepository.GetByNameAsync(request.Dto.SpecializationName);
        if (existingSpecialization != null && existingSpecialization.SpecializationId != request.SpecializationId)
        {
            _logger.LogWarning("⚠️ Ya existe otra especialización con el nombre: {SpecializationName}", request.Dto.SpecializationName);
            throw new InvalidOperationException($"Ya existe otra especialización con el nombre '{request.Dto.SpecializationName}'");
        }

        // Actualizar campos
        specialization.ProfessionId = request.Dto.ProfessionId;
        specialization.SpecializationName = request.Dto.SpecializationName;
        specialization.Description = request.Dto.Description;

        _unitOfWork.SpecializationRepository.Update(specialization);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Especialización actualizada: {SpecializationId} - {SpecializationName}", 
            specialization.SpecializationId, specialization.SpecializationName);

        var response = _mapper.Map<SpecializationResponseDto>(specialization);
        response.ProfessionName = profession.ProfessionName;
        response.TotalProfiles = specialization.ProfileSpecializations?.Count ?? 0;
        
        return response;
    }
}
