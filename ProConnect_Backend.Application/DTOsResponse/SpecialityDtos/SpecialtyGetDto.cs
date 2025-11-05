namespace ProConnect_Backend.Application.DTOsResponse.SpecialityDtos;

public class SpecialtyGetDto
{
    public uint SpecialtyId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 
}