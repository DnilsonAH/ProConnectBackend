using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    // Operaciones específicas de negocio para Review
    Task<IEnumerable<Review>> GetReviewsByProfessionalAsync(uint professionalId);
    Task<IEnumerable<Review>> GetReviewsByClientAsync(uint clientId);
    Task<double> GetAverageRatingByProfessionalAsync(uint professionalId);
    Task<Review?> GetReviewBySessionIdAsync(uint sessionId);
    Task<bool> HasClientReviewedSessionAsync(uint sessionId, uint clientId);
}
