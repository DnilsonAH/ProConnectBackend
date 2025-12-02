namespace ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;

public class ProfessionResultDto
{
    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Mantener compatibilidad con versiones anteriores
    public uint ProfessionId { get; set; }
    public string ProfessionName { get; set; } = string.Empty;
    public List<SpecializationResultDto> Specializations { get; set; } = new();
}