using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialization.Commands.CreateSpecialization;

public class CreateSpecializationHandler : IRequestHandler<CreateSpecializationCommand, SpecializationResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSpecializationHandler> _logger;

    public CreateSpecializationHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateSpecializationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SpecializationResponseDto> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Creando nueva especialización: {SpecializationName} en profesión {ProfessionId}", 
            request.Dto.SpecializationName, request.Dto.ProfessionId);

        // Validar que la profesión exista
        var profession = await _unitOfWork.ProfessionRepository.GetByIdAsync(request.Dto.ProfessionId);
        if (profession == null)
        {
            _logger.LogWarning("❌ Profesión no encontrada: {ProfessionId}", request.Dto.ProfessionId);
            throw new KeyNotFoundException($"No se encontró la profesión con ID {request.Dto.ProfessionId}");
        }

        // Validar que no exista una especialización con el mismo nombre
        var existingSpecialization = await _unitOfWork.SpecializationRepository.GetByNameAsync(request.Dto.SpecializationName);
        if (existingSpecialization != null)
        {
            _logger.LogWarning("⚠️ Ya existe una especialización con el nombre: {SpecializationName}", request.Dto.SpecializationName);
            throw new InvalidOperationException($"Ya existe una especialización con el nombre '{request.Dto.SpecializationName}'");
        }

        // Crear la entidad
        var specialization = _mapper.Map<Domain.Entities.Specialization>(request.Dto);
        
        await _unitOfWork.SpecializationRepository.AddAsync(specialization);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Especialización creada exitosamente: {SpecializationId} - {SpecializationName}", 
            specialization.SpecializationId, specialization.SpecializationName);

        var response = _mapper.Map<SpecializationResponseDto>(specialization);
        response.ProfessionName = profession.ProfessionName;
        response.TotalProfiles = 0; // Nueva especialización no tiene perfiles
        
        return response;
    }
}
