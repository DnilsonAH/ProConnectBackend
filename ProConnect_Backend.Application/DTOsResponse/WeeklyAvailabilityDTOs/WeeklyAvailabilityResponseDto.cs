namespace ProConnect_Backend.Application.DTOsResponse.WeeklyAvailabilityDTOs;

public class WeeklyAvailabilityResponseDto
{
    public uint WeeklyAvailabilityId { get; set; }
    public uint ProfessionalId { get; set; }
    public string WeekDay { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public bool? IsAvailable { get; set; }
}
