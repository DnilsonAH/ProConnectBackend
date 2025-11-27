using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetFilteredProfessionals;
using ProConnect_Backend.Application.UseCases.ProfileSpecialization.Commands.AssignSpecialization;
using ProConnect_Backend.Application.UseCases.ProfileSpecialization.Commands.RemoveSpecialization;
using ProConnect_Backend.Application.UseCases.ProfileSpecialization.Queries.GetProfileSpecializations;
using ProConnect_Backend.Application.UseCases.ProfileSpecialization.Queries.GetSpecializationProfiles;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionalProfileDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfileSpecializationDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileSpecializationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProfileSpecializationController> _logger;

    public ProfileSpecializationController(
        IMediator mediator,
        ILogger<ProfileSpecializationController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Asigna una especialización a un perfil profesional
    /// </summary>
    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignSpecializationRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ Datos inválidos en AssignSpecialization");
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState
                });
            }

            var command = new AssignSpecializationCommand(dto);
            var result = await _mediator.Send(command);

            _logger.LogInformation("✅ Especialización asignada: {ProfileSpecializationId}", result.ProfileSpecializationId);
            return Ok(new
            {
                success = true,
                message = "Especialización asignada exitosamente",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Perfil o especialización no encontrada");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida al asignar especialización");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al asignar especialización");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al asignar la especialización"
            });
        }
    }

    /// <summary>
    /// Remueve una especialización de un perfil profesional
    /// </summary>
    [HttpDelete("remove")]
    public async Task<IActionResult> Remove([FromQuery] uint profileId, [FromQuery] uint specializationId)
    {
        try
        {
            var command = new RemoveSpecializationCommand(profileId, specializationId);
            await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Especialización removida exitosamente"
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Asignación no encontrada");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al remover especialización");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al remover la especialización"
            });
        }
    }

    /// <summary>
    /// Obtiene todas las especializaciones de un perfil profesional
    /// </summary>
    [HttpGet("profile/{profileId}")]
    public async Task<IActionResult> GetProfileSpecializations(uint profileId)
    {
        try
        {
            var query = new GetProfileSpecializationsQuery(profileId);
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Perfil no encontrado: {ProfileId}", profileId);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener especializaciones del perfil {ProfileId}", profileId);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener las especializaciones"
            });
        }
    }

    /// <summary>
    /// Obtiene todos los perfiles que tienen una especialización específica
    /// </summary>
    [HttpGet("specialization/{specializationId}")]
    public async Task<IActionResult> GetSpecializationProfiles(uint specializationId)
    {
        try
        {
            var query = new GetSpecializationProfilesQuery(specializationId);
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Especialización no encontrada: {SpecializationId}", specializationId);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener perfiles con especialización {SpecializationId}", specializationId);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener los perfiles"
            });
        }
    }
    // Solo usuarios logueados pueden buscar
    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> SearchProfessionals([FromQuery] FilterProfessionalsRequestDto filters)
    {
        try
        {
            // Validacion de paginación básica
            if (filters.Page < 1 || filters.PageSize < 1)
            {
                return BadRequest(new { success = false, message = "Paginación inválida." });
            }

            var query = new GetFilteredProfessionalsQuery(filters);
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                message = "Resultados obtenidos exitosamente",
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar profesionales con filtros");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al procesar la búsqueda"
            });
        }
    }
}
