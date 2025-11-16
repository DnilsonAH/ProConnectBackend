using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.Specialization.Commands.CreateSpecialization;
using ProConnect_Backend.Application.UseCases.Specialization.Commands.UpdateSpecialization;
using ProConnect_Backend.Application.UseCases.Specialization.Commands.DeleteSpecialization;
using ProConnect_Backend.Application.UseCases.Specialization.Queries.GetAllSpecializations;
using ProConnect_Backend.Application.UseCases.Specialization.Queries.GetSpecializationById;
using ProConnect_Backend.Application.UseCases.Specialization.Queries.GetSpecializationsByProfession;
using ProConnect_Backend.Domain.DTOsRequest.SpecializationDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecializationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SpecializationController> _logger;

    public SpecializationController(
        IMediator mediator,
        ILogger<SpecializationController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Crea una nueva especialización
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSpecializationRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ Datos inválidos en CreateSpecialization");
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState
                });
            }

            var command = new CreateSpecializationCommand(dto);
            var result = await _mediator.Send(command);

            _logger.LogInformation("✅ Especialización creada: {SpecializationId}", result.SpecializationId);
            return CreatedAtAction(nameof(GetById), new { id = result.SpecializationId }, new
            {
                success = true,
                message = "Especialización creada exitosamente",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Profesión no encontrada");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida al crear especialización");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al crear especialización");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al crear la especialización"
            });
        }
    }

    /// <summary>
    /// Obtiene todas las especializaciones
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllSpecializationsQuery();
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener especializaciones");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener las especializaciones"
            });
        }
    }

    /// <summary>
    /// Obtiene una especialización por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        try
        {
            var query = new GetSpecializationByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("❌ Especialización no encontrada: {Id}", id);
                return NotFound(new
                {
                    success = false,
                    message = "Especialización no encontrada"
                });
            }

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener especialización {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener la especialización"
            });
        }
    }

    /// <summary>
    /// Obtiene todas las especializaciones de una profesión
    /// </summary>
    [HttpGet("profession/{professionId}")]
    public async Task<IActionResult> GetByProfession(uint professionId)
    {
        try
        {
            var query = new GetSpecializationsByProfessionQuery(professionId);
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Profesión no encontrada: {ProfessionId}", professionId);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener especializaciones de profesión {ProfessionId}", professionId);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener las especializaciones"
            });
        }
    }

    /// <summary>
    /// Actualiza una especialización existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, [FromBody] UpdateSpecializationRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ Datos inválidos en UpdateSpecialization");
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState
                });
            }

            var command = new UpdateSpecializationCommand(id, dto);
            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Especialización actualizada exitosamente",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Especialización o profesión no encontrada");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida al actualizar especialización");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al actualizar especialización {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al actualizar la especialización"
            });
        }
    }

    /// <summary>
    /// Elimina una especialización
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        try
        {
            var command = new DeleteSpecializationCommand(id);
            await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Especialización eliminada exitosamente"
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Especialización no encontrada: {Id}", id);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ No se puede eliminar la especialización: {Id}", id);
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al eliminar especialización {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al eliminar la especialización"
            });
        }
    }
}
