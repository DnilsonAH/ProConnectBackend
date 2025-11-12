using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IVerificationDocumentRepository : IGenericRepository<VerificationDocument>
{
    // Operaciones específicas de negocio para VerificationDocument
    Task<IEnumerable<VerificationDocument>> GetDocumentsByVerificationIdAsync(uint verificationId);
    Task<VerificationDocument?> GetDocumentByUrlAsync(string documentUrl);
    Task<int> GetDocumentCountByVerificationIdAsync(uint verificationId);
}
