using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IScheduledRepository : IGenericRepository<Scheduled>
{
    // Operaciones específicas de negocio para Scheduled
    Task<IEnumerable<Scheduled>> GetScheduledBySessionIdAsync(uint sessionId);
    Task<IEnumerable<Scheduled>> GetScheduledByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> HasConflictAsync(uint sessionId, DateTime scheduledDate);
}
