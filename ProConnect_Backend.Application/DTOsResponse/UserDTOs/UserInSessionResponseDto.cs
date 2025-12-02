namespace ProConnect_Backend.Application.DTOsResponse.UserDTOs;

public class UserInSessionResponseDto
{
    public uint UserId { get; set; }
    public string FirstName { get; set; }
    public string FirstSurname { get; set; }
    public string? PhotoUrl { get; set; }
}
