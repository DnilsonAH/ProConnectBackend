using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class RevokedTokenRepository : IRevokedTokenRepository
{
    private readonly ProConnectDbContext _context;

    public RevokedTokenRepository(ProConnectDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default)
    {
        return await _context.RevokedTokens
            .AnyAsync(rt => rt.TokenJti == jti, cancellationToken);
    }

    public async Task AddAsync(RevokedToken token, CancellationToken cancellationToken = default)
    {
        await _context.RevokedTokens.AddAsync(token, cancellationToken);
    }

    public async Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var expiredTokens = await _context.RevokedTokens
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        _context.RevokedTokens.RemoveRange(expiredTokens);
    }
}

