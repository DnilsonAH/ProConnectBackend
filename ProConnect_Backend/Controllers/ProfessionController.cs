using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.Profession.Commands.CreateProfession;
using ProConnect_Backend.Application.UseCases.Profession.Commands.UpdateProfession;
using ProConnect_Backend.Application.UseCases.Profession.Commands.DeleteProfession;
using ProConnect_Backend.Application.UseCases.Profession.Queries.GetAllProfessions;
using ProConnect_Backend.Application.UseCases.Profession.Queries.GetProfessionById;
using ProConnect_Backend.Application.UseCases.Profession.Queries.GetProfessionsByCategory;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProfessionController> _logger;

    public ProfessionController(
        IMediator mediator,
        ILogger<ProfessionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Crea una nueva profesión
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfessionRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ Datos inválidos en CreateProfession");
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState
                });
            }

            var command = new CreateProfessionCommand(dto);
            var result = await _mediator.Send(command);

            _logger.LogInformation("✅ Profesión creada: {ProfessionId}", result.ProfessionId);
            return CreatedAtAction(nameof(GetById), new { id = result.ProfessionId }, new
            {
                success = true,
                message = "Profesión creada exitosamente",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Categoría no encontrada");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida al crear profesión");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al crear profesión");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al crear la profesión"
            });
        }
    }

    /// <summary>
    /// Obtiene todas las profesiones
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllProfessionsQuery();
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener profesiones");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener las profesiones"
            });
        }
    }

    /// <summary>
    /// Obtiene una profesión por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        try
        {
            var query = new GetProfessionByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("❌ Profesión no encontrada: {Id}", id);
                return NotFound(new
                {
                    success = false,
                    message = "Profesión no encontrada"
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
            _logger.LogError(ex, "💥 Error al obtener profesión {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener la profesión"
            });
        }
    }

    /// <summary>
    /// Obtiene todas las profesiones de una categoría
    /// </summary>
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(uint categoryId)
    {
        try
        {
            var query = new GetProfessionsByCategoryQuery(categoryId);
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Categoría no encontrada: {CategoryId}", categoryId);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener profesiones de categoría {CategoryId}", categoryId);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener las profesiones"
            });
        }
    }

    /// <summary>
    /// Actualiza una profesión existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, [FromBody] UpdateProfessionRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ Datos inválidos en UpdateProfession");
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState
                });
            }

            var command = new UpdateProfessionCommand(id, dto);
            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Profesión actualizada exitosamente",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Profesión o categoría no encontrada");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida al actualizar profesión");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al actualizar profesión {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al actualizar la profesión"
            });
        }
    }

    /// <summary>
    /// Elimina una profesión
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        try
        {
            var command = new DeleteProfessionCommand(id);
            await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Profesión eliminada exitosamente"
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Profesión no encontrada: {Id}", id);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ No se puede eliminar la profesión: {Id}", id);
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al eliminar profesión {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al eliminar la profesión"
            });
        }
    }
}
