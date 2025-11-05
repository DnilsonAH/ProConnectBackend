using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.DTOsResponse.SpecialityDtos;
using ProConnect_Backend.Application.UseCases.Specialty.Command;
using ProConnect_Backend.Application.UseCases.Specialty.Query;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialtyController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public SpecialtyController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// Obtener todas las especialidades
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllSpecialtiesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SpecialtyDto specialtyDto)
    {
        var command = new CreateSpecialtyCommand()
        {
            SpecialtyDto = specialtyDto
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(uint specialtyId, SpecialtyDto SpecialtyDto)
    {
        var command = new UpdateSpecialtyCommand()
        {
            SpecialtyId = specialtyId,
            SpecialtyDto = SpecialtyDto
        };
        await _mediator.Send(command);
        return Ok("Especialidad actualizada correctamente");
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(uint specialtyId)
    {
        var command = new DeleteSpecialtyCommand()
        {
            SpecialtyId = specialtyId
        };
        await _mediator.Send(command);
        return Ok("Especialidad eliminada correctamente");
    }
}