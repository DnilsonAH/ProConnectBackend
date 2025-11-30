using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DomainEntities = ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Commands.UpdateProfessionalProfile;

public class UpdateProfessionalProfileHandler : IRequestHandler<UpdateProfessionalProfileCommand, ProfessionalProfileResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProfessionalProfileHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateProfessionalProfileHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateProfessionalProfileHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProfessionalProfileResponseDto> Handle(UpdateProfessionalProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando actualización de perfil profesional {ProfileId}", request.ProfileId);

        var entity = await _unitOfWork.ProfessionalProfileRepository.GetByIdAsync(request.ProfileId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Perfil profesional con ID {request.ProfileId} no encontrado");
        }

        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        var userRoleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null || !uint.TryParse(userIdClaim.Value, out uint userId))
        {
            throw new UnauthorizedAccessException("Usuario no autenticado");
        }


        // Validar permisos: Admin o dueño
        if (userRoleClaim?.Value != "Admin" && entity.UserId != userId)
        {
            throw new UnauthorizedAccessException("No tiene permisos para modificar este perfil");
        }

        // 4. Mapear cambios
        _mapper.Map(request.Dto, entity);

        // 5. Actualizar Especializaciones si se proporcionan
        if (request.Dto.SpecializationIds != null)
        {
            // Limpiar especializaciones existentes
            // Nota: Esto asume que ProfileSpecializations está incluido en la entidad. 
            // Si no, deberíamos cargarlo explícitamente o usar un método del repositorio para limpiar.
            // Para simplificar y asegurar consistencia, cargaremos las relaciones si no están.

            // En una implementación ideal, el repositorio debería manejar esto, pero aquí lo haremos manualmente
            // o asumiremos que el GetByIdAsync del repositorio incluye las relaciones necesarias o usamos lazy loading.
            // Dado que GenericRepository suele ser simple, es mejor asegurar la carga.

            // TODO: Asegurar que el repositorio cargue ProfileSpecializations o hacerlo aquí.
            // Por ahora, asumiremos que se pueden agregar y el contexto manejará la eliminación si modificamos la colección.
            // Sin embargo, EF Core necesita saber qué borrar. 

            // Estrategia segura: Borrar todas las relaciones existentes para este perfil y agregar las nuevas.
            var existingSpecs = await _unitOfWork.ProfileSpecializationRepository.GetByProfileIdAsync(entity.ProfileId);
            foreach (var spec in existingSpecs)
            {
                _unitOfWork.ProfileSpecializationRepository.Delete(spec);
            }

            // Agregar nuevas
            foreach (var specId in request.Dto.SpecializationIds)
            {
                var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(specId);
                if (specialization == null)
                {
                    throw new KeyNotFoundException($"Especialización con ID {specId} no encontrada");
                }

                await _unitOfWork.ProfileSpecializationRepository.AddAsync(new DomainEntities.ProfileSpecialization
                {
                    ProfileId = entity.ProfileId,
                    SpecializationId = specId
                });
            }
        }

        // 6. Guardar cambios
        _unitOfWork.ProfessionalProfileRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Perfil profesional actualizado exitosamente: {ProfileId}", entity.ProfileId);

        // Recargar entidad para devolver respuesta completa con especializaciones
        // O simplemente mapear lo que tenemos si no necesitamos devolver las specs actualizadas inmediatamente
        return _mapper.Map<ProfessionalProfileResponseDto>(entity);
    }
}
