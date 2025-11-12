using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class VerificationRepository : GenericRepository<Verification>, IVerificationRepository
{
    public VerificationRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Verification>> GetVerificationsByUserIdAsync(uint userId)
    {
        return await _dbContext.Verifications
            .Where(v => v.UserId == userId)
            .Include(v => v.VerificationDocuments)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Verification>> GetPendingVerificationsAsync()
    {
        return await _dbContext.Verifications
            .Where(v => v.Status == "pending")
            .Include(v => v.User)
            .Include(v => v.VerificationDocuments)
            .OrderBy(v => v.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Verification>> GetVerificationsByStatusAsync(string status)
    {
        return await _dbContext.Verifications
            .Where(v => v.Status == status)
            .Include(v => v.User)
            .Include(v => v.VerificationDocuments)
            .ToListAsync();
    }

    public async Task<Verification?> GetVerificationWithDocumentsAsync(uint verificationId)
    {
        return await _dbContext.Verifications
            .Include(v => v.User)
            .Include(v => v.VerificationDocuments)
            .FirstOrDefaultAsync(v => v.VerificationId == verificationId);
    }
}
