namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IRevokedTokenRepository
{
    Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default);
    Task AddAsync(Domain.Entities.RevokedToken token, CancellationToken cancellationToken = default);
    Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default);
}

