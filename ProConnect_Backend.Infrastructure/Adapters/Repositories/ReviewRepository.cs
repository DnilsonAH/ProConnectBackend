using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Review>> GetReviewsByProfessionalAsync(uint professionalId)
    {
        return await _dbContext.Reviews
            .Where(r => r.ProfessionalId == professionalId)
            .Include(r => r.Client)
            .Include(r => r.Session)
            .OrderByDescending(r => r.ReviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByClientAsync(uint clientId)
    {
        return await _dbContext.Reviews
            .Where(r => r.ClientId == clientId)
            .Include(r => r.Professional)
            .Include(r => r.Session)
            .OrderByDescending(r => r.ReviewDate)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingByProfessionalAsync(uint professionalId)
    {
        var reviews = await _dbContext.Reviews
            .Where(r => r.ProfessionalId == professionalId)
            .ToListAsync();

        if (!reviews.Any())
            return 0;

        return reviews.Average(r => (double)r.Rating);
    }

    public async Task<Review?> GetReviewBySessionIdAsync(uint sessionId)
    {
        return await _dbContext.Reviews
            .FirstOrDefaultAsync(r => r.SessionId == sessionId);
    }

    public async Task<bool> HasClientReviewedSessionAsync(uint sessionId, uint clientId)
    {
        return await _dbContext.Reviews
            .AnyAsync(r => r.SessionId == sessionId && r.ClientId == clientId);
    }
}
