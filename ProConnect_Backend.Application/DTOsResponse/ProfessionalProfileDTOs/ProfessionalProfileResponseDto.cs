using ProConnect_Backend.Application.DTOsResponse.UserDTOs;

namespace ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;

public class ProfessionalProfileResponseDto
{
    public uint ProfileId { get; set; }
    public uint UserId { get; set; }
    public UserResponseDto? User { get; set; }
    public uint ProfessionId { get; set; }
    public ProfessionResultDto? Profession { get; set; }
    public string? Bio { get; set; }
    public double HourlyRate { get; set; }
    public int YearsOfExperience { get; set; }
    public bool IsVerified { get; set; }
    public string? ProfileImageUrl { get; set; }
    public List<SpecializationResultDto> Specializations { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    
    // Mantener compatibilidad con versiones anteriores
    public string? Experience { get; set; }
    public string? Presentation { get; set; }
}
