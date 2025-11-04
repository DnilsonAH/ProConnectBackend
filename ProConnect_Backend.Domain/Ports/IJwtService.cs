using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
