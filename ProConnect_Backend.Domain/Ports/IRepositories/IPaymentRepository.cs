using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    // Operaciones específicas de negocio para Payment
    Task<Payment?> GetPaymentBySessionIdAsync(uint sessionId);
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
    Task<IEnumerable<Payment>> GetPaymentsByUserAsync(uint userId);
    Task<decimal> GetTotalRevenueByProfessionalAsync(uint professionalId);
}
