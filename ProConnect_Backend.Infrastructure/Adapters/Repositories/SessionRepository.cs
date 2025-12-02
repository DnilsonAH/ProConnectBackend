using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Domain.Ports.IServices;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    private readonly ITimeZoneConverter _timeZoneConverter;

    public SessionRepository(ProConnectDbContext dbContext, ITimeZoneConverter timeZoneConverter) : base(dbContext)
    {
        _timeZoneConverter = timeZoneConverter;
    }

    /// <summary>
    /// Devuelve una lista de sesiones para un cliente específico.
    /// </summary>
    /// <returns>
    /// Una lista de sesiones con la siguiente estructura:
    /// <code>
    /// [
    ///   {
    ///     "sessionId": 1,
    ///     "startDate": "2024-05-20T10:00:00Z",
    ///     "endDate": "2024-05-20T11:00:00Z",
    ///     "professionalId": 2,
    ///     "clientId": 1,
    ///     "meetUrl": "https://meet.example.com/session1",
    ///     "status": "Completed",
    ///     "professional": { "userId": 2, "firstName": "Professional", "firstSurname": "User" /* ... */ },
    ///     "scheduleds": [ { "availabilityId": 1, "startDate": "2024-05-20T10:00:00Z", "endDate": "2024-05-20T11:00:00Z" } ]
    ///   }
    /// ]
    /// </code>
    /// </returns>
    public async Task<IEnumerable<Session>> GetSessionsByClientAsync(uint clientId)
    {
        return await _dbContext.Sessions
            .Where(s => s.ClientId == clientId)
            .Include(s => s.Professional)
            .Include(s => s.Scheduleds)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    /// <summary>
    /// Devuelve una lista de sesiones para un profesional específico.
    /// </summary>
    /// <returns>
    /// Una lista de sesiones con la siguiente estructura:
    /// <code>
    /// [
    ///   {
    ///     "sessionId": 1,
    ///     "startDate": "2024-05-20T10:00:00Z",
    ///     "endDate": "2024-05-20T11:00:00Z",
    ///     "professionalId": 2,
    ///     "clientId": 1,
    ///     "meetUrl": "https://meet.example.com/session1",
    ///     "status": "Completed",
    ///     "client": { "userId": 1, "firstName": "Client", "firstSurname": "User" /* ... */ },
    ///     "scheduleds": [ { "availabilityId": 1, "startDate": "2024-05-20T10:00:00Z", "endDate": "2024-05-20T11:00:00Z" } ]
    ///   }
    /// ]
    /// </code>
    /// </returns>
    public async Task<IEnumerable<Session>> GetSessionsByProfessionalAsync(uint professionalId)
    {
        return await _dbContext.Sessions
            .Where(s => s.ProfessionalId == professionalId)
            .Include(s => s.Client)
            .Include(s => s.Scheduleds)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    /// <summary>
    /// Devuelve una lista de sesiones con un estado específico.
    /// </summary>
    /// <returns>
    /// Una lista de sesiones con la siguiente estructura:
    /// <code>
    /// [
    ///   {
    ///     "sessionId": 1,
    ///     "startDate": "2024-05-20T10:00:00Z",
    ///     "endDate": "2024-05-20T11:00:00Z",
    ///     "professionalId": 2,
    ///     "clientId": 1,
    ///     "meetUrl": "https://meet.example.com/session1",
    ///     "status": "Scheduled",
    ///     "client": { "userId": 1, "firstName": "Client", "firstSurname": "User" /* ... */ },
    ///     "professional": { "userId": 2, "firstName": "Professional", "firstSurname": "User" /* ... */ }
    ///   }
    /// ]
    /// </code>
    /// </returns>
    public async Task<IEnumerable<Session>> GetSessionsByStatusAsync(string status)
    {
        return await _dbContext.Sessions
            .Where(s => s.Status == status)
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .ToListAsync();
    }

    /// <summary>
    /// Devuelve una sesión con todos sus detalles.
    /// </summary>
    /// <returns>
    /// Un objeto de sesión con la siguiente estructura, o null si no se encuentra:
    /// <code>
    /// {
    ///   "sessionId": 1,
    ///   "startDate": "2024-05-20T10:00:00Z",
    ///   "endDate": "2024-05-20T11:00:00Z",
    ///   "professionalId": 2,
    ///   "clientId": 1,
    ///   "meetUrl": "https://meet.example.com/session1",
    ///   "status": "Completed",
    ///   "client": { "userId": 1, "firstName": "Client", "firstSurname": "User" /* ... */ },
    ///   "professional": { "userId": 2, "firstName": "Professional", "firstSurname": "User" /* ... */ },
    ///   "scheduleds": [ { "availabilityId": 1, "startDate": "2024-05-20T10:00:00Z", "endDate": "2024-05-20T11:00:00Z" } ],
    ///   "payments": [ { "paymentId": 1, "totalAmount": 100.00, "status": "Paid", "paymentDate": "2024-05-19T12:00:00Z" } ],
    ///   "reviews": [ { "reviewId": 1, "rating": 5, "comment": "Excellent session!", "reviewDate": "2024-05-21T14:00:00Z" } ]
    /// }
    /// </code>
    /// </returns>
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

    /// <summary>
    /// Devuelve una lista de las próximas sesiones para un usuario específico.
    /// </summary>
    /// <returns>
    /// Una lista de sesiones con la siguiente estructura:
    /// <code>
    /// [
    ///   {
    ///     "sessionId": 1,
    ///     "startDate": "2024-05-20T10:00:00Z",
    ///     "endDate": "2024-05-20T11:00:00Z",
    ///     "professionalId": 2,
    ///     "clientId": 1,
    ///     "meetUrl": "https://meet.example.com/session1",
    ///     "status": "Scheduled",
    ///     "client": { "userId": 1, "firstName": "Client", "firstSurname": "User" /* ... */ },
    ///     "professional": { "userId": 2, "firstName": "Professional", "firstSurname": "User" /* ... */ },
    ///     "scheduleds": [ { "availabilityId": 1, "startDate": "2024-05-20T10:00:00Z", "endDate": "2024-05-20T11:00:00Z" } ]
    ///   }
    /// ]
    /// </code>
    /// </returns>
    public async Task<IEnumerable<Session>> GetUpcomingSessionsAsync(uint userId)
    {
        var now = _timeZoneConverter.GetLocalTimeInColombiaPeru().Date;
        return await _dbContext.Sessions
            .Where(s => (s.ClientId == userId || s.ProfessionalId == userId)
                     && s.StartDate.Date >= now)
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .Include(s => s.Scheduleds)
            .ToListAsync();
    }

    
    /// <summary>
    /// Devuelve una lista de sesiones pasadas para un usuario específico.
    /// </summary>
    /// <returns>
    /// Una lista de sesiones con la siguiente estructura:
    /// <code>
    /// [
    ///   {
    ///     "sessionId": 1,
    ///     "startDate": "2024-05-20T10:00:00Z",
    ///     "endDate": "2024-05-20T11:00:00Z",
    ///     "professionalId": 2,
    ///     "clientId": 1,
    ///     "meetUrl": "https://meet.example.com/session1",
    ///     "status": "Completed",
    ///     "client": { "userId": 1, "firstName": "Client", "firstSurname": "User" /* ... */ },
    ///     "professional": { "userId": 2, "firstName": "Professional", "firstSurname": "User" /* ... */ },
    ///     "scheduleds": [ { "availabilityId": 1, "startDate": "2024-05-20T10:00:00Z", "endDate": "2024-05-20T11:00:00Z" } ]
    ///   }
    /// ]
    /// </code>
    /// </returns>
    public async Task<IEnumerable<Session>> GetPastSessionsAsync(uint userId)
    {
        var now = _timeZoneConverter.GetLocalTimeInColombiaPeru();
        return await _dbContext.Sessions
            .Where(s => (s.ClientId == userId || s.ProfessionalId == userId) 
                     && s.StartDate < now)
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .Include(s => s.Scheduleds)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Session>> GetAllWithDetailsAsync()
    {
        return await _dbContext.Sessions
            .Include(s => s.Client)
            .Include(s => s.Professional)
            .ToListAsync();
    }
}
