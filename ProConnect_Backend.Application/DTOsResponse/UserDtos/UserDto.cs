namespace ProConnect_Backend.Application.DTOsResponse.UserDtos;

public class UserDto
{
    public int UserId { get; set; } 
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!;
}