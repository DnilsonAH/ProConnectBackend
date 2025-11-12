using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    public SessionRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Session>> GetSessionsByClientAsync(uint clientId)
    {
        return await _dbContext.Sessions
            .Where(s => s.ClientId == clientId)
            .Include(s => s.Professional)
            .Include(s => s.Scheduleds)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetSessionsByProfessionalAsync(uint professionalId)
    {
        return await _dbContext.Sessions
            .Where(s => s.ProfessionalId == professionalId)
            .Include(s => s.Client)
            .Include(s => s.Scheduleds)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetSessionsByStatusAsync(string status)
    {
        return await _dbContext.Sessions
            .Where(s => s.Status == status)
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .ToListAsync();
    }

    public async Task<Session?> GetSessionWithDetailsAsync(uint sessionId)
    {
        return await _dbContext.Sessions
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .Include(s => s.Scheduleds)
            .Include(s => s.Payments)
            .Include(s => s.Reviews)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
    }

    public async Task<IEnumerable<Session>> GetUpcomingSessionsAsync(uint userId)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.Sessions
            .Where(s => (s.ClientId == userId || s.ProfessionalId == userId) 
                     && s.Scheduleds.Any(sch => sch.StartDate > now))
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .Include(s => s.Scheduleds)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetPastSessionsAsync(uint userId)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.Sessions
            .Where(s => (s.ClientId == userId || s.ProfessionalId == userId) 
                     && s.Scheduleds.Any(sch => sch.StartDate < now))
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .Include(s => s.Scheduleds)
            .ToListAsync();
    }
}
