using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.SessionDTOs;

public class CreateSessionDto
{
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public uint ProfessionalId { get; set; }

    [Required]
    public uint ClientId { get; set; }

    public string? MeetUrl { get; set; }

    [Required]
    public string Status { get; set; }
}
