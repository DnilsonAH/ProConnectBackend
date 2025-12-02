namespace ProConnect_Backend.Domain.DTOsRequest.SessionDTOs;

public class UpdateSessionDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public uint? ProfessionalId { get; set; }
    public uint? ClientId { get; set; }
    public string? MeetUrl { get; set; }
    public string? Status { get; set; }
}
