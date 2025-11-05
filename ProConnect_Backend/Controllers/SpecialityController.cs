using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProConnect_Backend.Application.UseCases.Specialty.Query;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialityController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public SpecialityController(IMediator mediator)
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
}