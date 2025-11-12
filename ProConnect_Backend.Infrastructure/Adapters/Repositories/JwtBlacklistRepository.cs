using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class JwtBlacklistRepository : GenericRepository<JwtBlacklist>, IJwtBlacklistRepository
{
    public JwtBlacklistRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsTokenRevokedAsync(string jti)
    {
        return await _dbContext.JwtBlacklists
            .AnyAsync(t => t.Token == jti);
    }

    public async Task AddTokenAsync(string jti, uint userId, DateTime expiresAt)
    {
        var blacklistedToken = new JwtBlacklist
        {
            Token = jti,
            UserId = userId,
            ExpiresAt = expiresAt
        };

        await _dbContext.JwtBlacklists.AddAsync(blacklistedToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveExpiredTokensAsync()
    {
        var expiredTokens = await _dbContext.JwtBlacklists
            .Where(t => t.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        _dbContext.JwtBlacklists.RemoveRange(expiredTokens);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<JwtBlacklist>> GetTokensByUserIdAsync(uint userId)
    {
        return await _dbContext.JwtBlacklists
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }
}
