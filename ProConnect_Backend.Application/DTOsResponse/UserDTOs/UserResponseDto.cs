namespace ProConnect_Backend.Application.DTOsResponse.UserDTOs;

public class UserResponseDto
{
    public uint UserId { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? FirstSurname { get; set; }
    public string? SecondSurname { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Role { get; set; }
}
