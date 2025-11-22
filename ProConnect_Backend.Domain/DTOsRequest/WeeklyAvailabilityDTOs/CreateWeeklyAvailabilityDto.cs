using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.WeeklyAvailabilityDTOs;

public class CreateWeeklyAvailabilityDto
{
    [Required]
    public string WeekDay { get; set; } = null!;

    [Required]
    public string StartTime { get; set; } = null!;

    [Required]
    public string EndTime { get; set; } = null!;

    public bool? IsAvailable { get; set; } = true;
}
