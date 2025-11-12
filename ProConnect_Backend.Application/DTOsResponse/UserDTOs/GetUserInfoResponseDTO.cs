namespace ProConnect_Backend.Application.DTOsResponse.UserDTOs;

public class GetUserInfoResponseDto
{
    public uint UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Country { get; set; }
    public DateTime RegistrationDate { get; set; }
}
