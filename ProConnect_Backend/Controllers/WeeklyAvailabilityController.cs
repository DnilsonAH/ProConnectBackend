using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.CreateWeeklyAvailability;
using ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.DeleteWeeklyAvailability;
using ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.UpdateWeeklyAvailability;
using ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.UpdateWeeklyAvailabilityAdmin;
using ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.DeleteWeeklyAvailabilityAdmin;
using ProConnect_Backend.Application.UseCases.WeeklyAvailability.Queries.GetWeeklyAvailabilityByProfessional;
using ProConnect_Backend.Domain.DTOsRequest.WeeklyAvailabilityDTOs;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/weekly-availability")]
public class WeeklyAvailabilityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WeeklyAvailabilityController> _logger;

    public WeeklyAvailabilityController(IMediator mediator, ILogger<WeeklyAvailabilityController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new weekly availability slot for the authenticated professional.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateWeeklyAvailabilityDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var command = new CreateWeeklyAvailabilityCommand(dto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByProfessional), new { professionalId = result.ProfessionalId }, new { success = true, message = "Weekly availability created successfully.", data = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating weekly availability.");
            return StatusCode(500, new { success = false, message = "An error occurred while creating weekly availability." });
        }
    }

    /// <summary>
    /// Updates an existing weekly availability slot.
    /// </summary>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateWeeklyAvailabilityDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var command = new UpdateWeeklyAvailabilityCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(new { success = true, message = "Weekly availability updated successfully.", data = result });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating weekly availability.");
            return StatusCode(500, new { success = false, message = "An error occurred while updating weekly availability." });
        }
    }

    /// <summary>
    /// Deletes a weekly availability slot.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(uint id)
    {
        try
        {
            var command = new DeleteWeeklyAvailabilityCommand(id);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Weekly availability deleted successfully." });
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
            _logger.LogError(ex, "Error deleting weekly availability.");
            return StatusCode(500, new { success = false, message = "An error occurred while deleting weekly availability." });
        }
    }

    /// <summary>
    /// Gets all weekly availability slots for a specific professional.
    /// </summary>
    [HttpGet("professional/{professionalId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByProfessional(uint professionalId)
    {
        try
        {
            var query = new GetWeeklyAvailabilityByProfessionalQuery(professionalId);
            var result = await _mediator.Send(query);
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving weekly availability.");
            return StatusCode(500, new { success = false, message = "An error occurred while retrieving weekly availability." });
        }
    }

    /// <summary>
    /// Updates a weekly availability slot for a specific professional (Admin only).
    /// </summary>
    [HttpPut("admin/{professionalId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAdmin(uint professionalId, [FromBody] UpdateWeeklyAvailabilityDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var command = new UpdateWeeklyAvailabilityAdminCommand(professionalId, dto);
            var result = await _mediator.Send(command);
            return Ok(new { success = true, message = "Weekly availability updated successfully by Admin.", data = result });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating weekly availability by Admin.");
            return StatusCode(500, new { success = false, message = "An error occurred while updating weekly availability." });
        }
    }

    /// <summary>
    /// Deletes a weekly availability slot for a specific professional (Admin only).
    /// </summary>
    [HttpDelete("admin/{professionalId}/{weeklyAvailabilityId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAdmin(uint professionalId, uint weeklyAvailabilityId)
    {
        try
        {
            var command = new DeleteWeeklyAvailabilityAdminCommand(professionalId, weeklyAvailabilityId);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Weekly availability deleted successfully by Admin." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { success = false, message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting weekly availability by Admin.");
            return StatusCode(500, new { success = false, message = "An error occurred while deleting weekly availability." });
        }
    }
}
