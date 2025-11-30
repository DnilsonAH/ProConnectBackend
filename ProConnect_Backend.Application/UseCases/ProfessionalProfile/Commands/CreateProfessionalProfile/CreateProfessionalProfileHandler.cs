using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;
using DomainEntities = ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Commands.CreateProfessionalProfile;

public class CreateProfessionalProfileHandler : IRequestHandler<CreateProfessionalProfileCommand, ProfessionalProfileResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProfessionalProfileHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateProfessionalProfileHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateProfessionalProfileHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProfessionalProfileResponseDto> Handle(CreateProfessionalProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando creación de perfil profesional");

        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !uint.TryParse(userIdClaim.Value, out uint userId))
        {
            throw new UnauthorizedAccessException("Usuario no autenticado o ID inválido");
        }


        // Verificar si ya tiene perfil
        var existingProfile = await _unitOfWork.ProfessionalProfileRepository.GetByUserIdAsync(userId);
        if (existingProfile != null)
        {
            throw new InvalidOperationException("El usuario ya tiene un perfil profesional");
        }

        // 3. Mapear DTO a Entidad
        var entity = _mapper.Map<DomainEntities.ProfessionalProfile>(request.Dto);
        entity.UserId = userId;

        // 4. Manejar Especializaciones
        if (request.Dto.SpecializationIds.Any())
        {
            // Validar que las especializaciones existan
            foreach (var specId in request.Dto.SpecializationIds)
            {
                var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(specId);
                if (specialization == null)
                {
                    throw new KeyNotFoundException($"Especialización con ID {specId} no encontrada");
                }

                entity.ProfileSpecializations.Add(new DomainEntities.ProfileSpecialization
                {
                    SpecializationId = specId
                });
            }
        }

        // 5. Guardar en BD
        await _unitOfWork.ProfessionalProfileRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Perfil profesional creado exitosamente con ID: {ProfileId}", entity.ProfileId);

        return _mapper.Map<ProfessionalProfileResponseDto>(entity);
    }
}
