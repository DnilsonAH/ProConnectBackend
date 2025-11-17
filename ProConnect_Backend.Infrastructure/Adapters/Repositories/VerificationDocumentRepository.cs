using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class VerificationDocumentRepository : GenericRepository<VerificationDocument>, IVerificationDocumentRepository
{
    public VerificationDocumentRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<VerificationDocument>> GetDocumentsByVerificationIdAsync(uint verificationId)
    {
        return await _dbContext.VerificationDocuments
            .Where(d => d.VerificationId == verificationId)
            .ToListAsync();
    }

    public async Task<VerificationDocument?> GetDocumentByUrlAsync(string documentUrl)
    {
        return await _dbContext.VerificationDocuments
            .FirstOrDefaultAsync(d => d.FileUrl == documentUrl);
    }

    public async Task<int> GetDocumentCountByVerificationIdAsync(uint verificationId)
    {
        return await _dbContext.VerificationDocuments
            .CountAsync(d => d.VerificationId == verificationId);
    }
}
