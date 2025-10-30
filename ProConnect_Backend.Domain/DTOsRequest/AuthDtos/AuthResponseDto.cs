namespace ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

public class AuthResponseDto
{
    public uint UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = new List<string>();
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
