using ProConnect_Backend.Application.DTOsResponse.UserDTOs;

namespace ProConnect_Backend.Application.DTOsResponse.SessionDTOs;

public class SessionResponseDto
{
    public uint SessionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public UserInSessionResponseDto Professional { get; set; }
    public UserInSessionResponseDto Client { get; set; }
    public string? MeetUrl { get; set; }
    public string Status { get; set; }
}
