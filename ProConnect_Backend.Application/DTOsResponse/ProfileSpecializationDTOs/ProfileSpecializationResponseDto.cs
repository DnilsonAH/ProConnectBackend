namespace ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;

public class ProfileSpecializationResponseDto
{
    public uint ProfileSpecializationId { get; set; }
    public uint ProfileId { get; set; }
    public uint SpecializationId { get; set; }
    public string SpecializationName { get; set; } = null!;
    public string ProfessionName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
