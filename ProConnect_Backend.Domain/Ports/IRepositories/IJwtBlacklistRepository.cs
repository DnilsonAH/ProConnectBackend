using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IJwtBlacklistRepository : IGenericRepository<JwtBlacklist>
{
    // Operaciones específicas de negocio para JWT Blacklist
    Task<bool> IsTokenRevokedAsync(string jti);
    Task AddTokenAsync(string jti, uint userId, DateTime expiresAt);
    Task RemoveExpiredTokensAsync();
    Task<IEnumerable<JwtBlacklist>> GetTokensByUserIdAsync(uint userId);
}
