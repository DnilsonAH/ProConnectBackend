namespace ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;

public class ProfessionalSearchResultDto
{
    public uint UserId { get; set; }
    public uint ProfileId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string FirstSurname { get; set; } = string.Empty;
    public string SecondSurname { get; set; } = string.Empty;
    public string Presentation { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    
    // Lista jerarquica Profesion->Especializacion
    public List<ProfessionResultDto> Professions { get; set; } = new();
}