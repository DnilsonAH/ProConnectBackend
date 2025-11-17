using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IVerificationRepository : IGenericRepository<Verification>
{
    // Operaciones específicas de negocio para Verification
    Task<IEnumerable<Verification>> GetVerificationsByUserIdAsync(uint userId);
    Task<IEnumerable<Verification>> GetPendingVerificationsAsync();
    Task<IEnumerable<Verification>> GetVerificationsByStatusAsync(string status);
    Task<Verification?> GetVerificationWithDocumentsAsync(uint verificationId);
}
