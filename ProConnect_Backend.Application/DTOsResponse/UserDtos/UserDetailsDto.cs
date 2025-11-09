namespace ProConnect_Backend.Application.DTOsResponse.UserDtos;

public class UserDetailsDto
{
    public uint Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = null!;
    public string? PhotoUrl { get; set; }
    public DateTime RegistrationDate { get; set; }
}