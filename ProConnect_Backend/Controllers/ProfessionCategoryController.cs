using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.CreateProfessionCategory;
using ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.UpdateProfessionCategory;
using ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.DeleteProfessionCategory;
using ProConnect_Backend.Application.UseCases.ProfessionCategory.Queries.GetAllProfessionCategories;
using ProConnect_Backend.Application.UseCases.ProfessionCategory.Queries.GetProfessionCategoryById;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionCategoryDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionCategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProfessionCategoryController> _logger;

    public ProfessionCategoryController(
        IMediator mediator,
        ILogger<ProfessionCategoryController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Crea una nueva categoría de profesión
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfessionCategoryRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ Datos inválidos en CreateProfessionCategory");
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState
                });
            }

            var command = new CreateProfessionCategoryCommand(dto);
            var result = await _mediator.Send(command);

            _logger.LogInformation("✅ Categoría creada: {CategoryId}", result.CategoryId);
            return CreatedAtAction(nameof(GetById), new { id = result.CategoryId }, new
            {
                success = true,
                message = "Categoría creada exitosamente",
                data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida al crear categoría");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al crear categoría");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al crear la categoría"
            });
        }
    }

    /// <summary>
    /// Obtiene todas las categorías de profesión
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllProfessionCategoriesQuery();
            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al obtener categorías");
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener las categorías"
            });
        }
    }

    /// <summary>
    /// Obtiene una categoría por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        try
        {
            var query = new GetProfessionCategoryByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("❌ Categoría no encontrada: {Id}", id);
                return NotFound(new
                {
                    success = false,
                    message = "Categoría no encontrada"
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
            _logger.LogError(ex, "💥 Error al obtener categoría {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al obtener la categoría"
            });
        }
    }

    /// <summary>
    /// Actualiza una categoría existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, [FromBody] UpdateProfessionCategoryRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ Datos inválidos en UpdateProfessionCategory");
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState
                });
            }

            var command = new UpdateProfessionCategoryCommand(id, dto);
            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Categoría actualizada exitosamente",
                data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Categoría no encontrada: {Id}", id);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida al actualizar categoría");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al actualizar categoría {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al actualizar la categoría"
            });
        }
    }

    /// <summary>
    /// Elimina una categoría
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        try
        {
            var command = new DeleteProfessionCategoryCommand(id);
            await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Categoría eliminada exitosamente"
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "❌ Categoría no encontrada: {Id}", id);
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ No se puede eliminar la categoría: {Id}", id);
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error al eliminar categoría {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "Error interno al eliminar la categoría"
            });
        }
    }
}
