using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.ProfessionalProfile.Commands.CreateProfessionalProfile;
using ProConnect_Backend.Application.UseCases.ProfessionalProfile.Commands.DeleteProfessionalProfile;
using ProConnect_Backend.Application.UseCases.ProfessionalProfile.Commands.UpdateProfessionalProfile;
using ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetAllProfessionalProfiles;
using ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetFilteredProfessionals;
using ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetProfessionalProfileById;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionalProfileDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionalProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProfessionalProfileController> _logger;

    public ProfessionalProfileController(IMediator mediator, ILogger<ProfessionalProfileController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Crea un nuevo perfil profesional
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateProfessionalProfileDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });
            }

            var command = new CreateProfessionalProfileCommand(dto);
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.ProfileId }, new
            {
                success = true,
                message = "Perfil profesional creado exitosamente",
                data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operación inválida al crear perfil");
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear perfil profesional");
            return StatusCode(500, new { success = false, message = "Error interno al crear el perfil" });
        }
    }

    /// <summary>
    /// Obtiene un perfil profesional por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        try
        {
            var query = new GetProfessionalProfileByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { success = false, message = "Perfil profesional no encontrado" });
            }

            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener perfil profesional {Id}", id);
            return StatusCode(500, new { success = false, message = "Error interno al obtener el perfil" });
        }
    }

    /// <summary>
    /// Obtiene todos los perfiles profesionales
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllProfessionalProfilesQuery();
            var result = await _mediator.Send(query);

            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener perfiles profesionales");
            return StatusCode(500, new { success = false, message = "Error interno al obtener los perfiles" });
        }
    }

    /// <summary>
    /// Actualiza un perfil profesional
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(uint id, [FromBody] UpdateProfessionalProfileDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });
            }

            var command = new UpdateProfessionalProfileCommand(id, dto);
            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Perfil profesional actualizado exitosamente",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar perfil profesional {Id}", id);
            return StatusCode(500, new { success = false, message = "Error interno al actualizar el perfil" });
        }
    }

    /// <summary>
    /// Elimina un perfil profesional
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(uint id)
    {
        try
        {
            var command = new DeleteProfessionalProfileCommand(id);
            await _mediator.Send(command);

            return Ok(new { success = true, message = "Perfil profesional eliminado exitosamente" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar perfil profesional {Id}", id);
            return StatusCode(500, new { success = false, message = "Error interno al eliminar el perfil" });
        }
    }

    /// <summary>
    /// Busca profesionales con filtros (Paginado)
    /// </summary>
    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] FilterProfessionalsRequestDto filters)
    {
        try
        {
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
            return StatusCode(500, new { success = false, message = "Error interno al procesar la búsqueda" });
        }
    }
}
