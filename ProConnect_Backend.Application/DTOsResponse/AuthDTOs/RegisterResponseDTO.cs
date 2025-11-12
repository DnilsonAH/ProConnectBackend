namespace ProConnect_Backend.Application.DTOsResponse.AuthDTOs;

public class RegisterResponseDto
{
    public uint UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public string Token { get; set; } = string.Empty;
}
