using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface ISessionRepository : IGenericRepository<Session>
{
    // Operaciones específicas de negocio para Session
    Task<IEnumerable<Session>> GetSessionsByClientAsync(uint clientId);
    Task<IEnumerable<Session>> GetSessionsByProfessionalAsync(uint professionalId);
    Task<IEnumerable<Session>> GetSessionsByStatusAsync(string status);
    Task<Session?> GetSessionWithDetailsAsync(uint sessionId);
    Task<IEnumerable<Session>> GetUpcomingSessionsAsync(uint userId);
    Task<IEnumerable<Session>> GetPastSessionsAsync(uint userId);
}
