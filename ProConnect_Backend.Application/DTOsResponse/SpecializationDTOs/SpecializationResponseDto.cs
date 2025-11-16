namespace ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;

public class SpecializationResponseDto
{
    public uint SpecializationId { get; set; }
    public uint ProfessionId { get; set; }
    public string ProfessionName { get; set; } = null!;
    public string SpecializationName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TotalProfiles { get; set; }
}
