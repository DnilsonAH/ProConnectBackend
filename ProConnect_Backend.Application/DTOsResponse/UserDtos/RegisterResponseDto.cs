namespace ProConnect_Backend.Application.DTOsResponse.UserDtos;

public class RegisterResponseDto
{
    public uint Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = null!;
}
