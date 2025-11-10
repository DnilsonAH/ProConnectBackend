namespace ProConnect_Backend.Domain.Entities;

public class RevokedToken
{
    public int Id { get; set; }
    public string TokenJti { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime RevokedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}

