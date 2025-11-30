using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.WeeklyAvailabilityDTOs;

public class UpdateWeeklyAvailabilityDto
{
    [Required]
    public uint WeeklyAvailabilityId { get; set; }

    public string? WeekDay { get; set; }

    public string? StartTime { get; set; }

    public string? EndTime { get; set; }

    public bool? IsAvailable { get; set; }
}
