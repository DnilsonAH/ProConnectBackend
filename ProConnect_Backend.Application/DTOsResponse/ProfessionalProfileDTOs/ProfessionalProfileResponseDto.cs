namespace ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;

public class ProfessionalProfileResponseDto
{
    public uint ProfileId { get; set; }
    public uint UserId { get; set; }
    public string? Experience { get; set; }
    public string? Presentation { get; set; }
    public List<SpecializationResultDto> Specializations { get; set; } = new();
}
