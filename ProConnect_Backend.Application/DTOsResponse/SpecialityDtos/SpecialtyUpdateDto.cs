namespace ProConnect_Backend.Application.DTOsResponse.SpecialityDtos;

public class SpecialtyUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}