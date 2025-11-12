namespace ProConnect_Backend.Domain.Ports.IServices;

public interface IJwtTokenService
{
    string GenerateToken(uint userId, string email, string role);
    bool ValidateToken(string token);
    (string jti, uint userId, DateTime expiresAt)? ParseToken(string token);
}
