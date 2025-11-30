namespace ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;

public class ProfessionResultDto
{
    public uint ProfessionId { get; set; }
    public string ProfessionName { get; set; } = string.Empty;
    public List<SpecializationResultDto> Specializations { get; set; } = new();
}