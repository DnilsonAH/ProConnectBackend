using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ScheduledRepository : GenericRepository<Scheduled>, IScheduledRepository
{
    public ScheduledRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Scheduled>> GetScheduledBySessionIdAsync(uint sessionId)
    {
        return await _dbContext.Scheduleds
            .Where(s => s.SessionId == sessionId)
            .OrderBy(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Scheduled>> GetScheduledByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbContext.Scheduleds
            .Where(s => s.StartDate >= startDate && s.StartDate <= endDate)
            .Include(s => s.Session)
            .OrderBy(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<bool> HasConflictAsync(uint sessionId, DateTime scheduledDate)
    {
        return await _dbContext.Scheduleds
            .AnyAsync(s => s.SessionId == sessionId && s.StartDate == scheduledDate);
    }
}
