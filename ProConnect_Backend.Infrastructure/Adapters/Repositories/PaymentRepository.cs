using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Payment?> GetPaymentBySessionIdAsync(uint sessionId)
    {
        return await _dbContext.Payments
            .FirstOrDefaultAsync(p => p.SessionId == sessionId);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
    {
        return await _dbContext.Payments
            .Where(p => p.Status == status)
            .Include(p => p.Session)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(uint userId)
    {
        return await _dbContext.Payments
            .Where(p => p.Session.ClientId == userId || p.Session.ProfessionalId == userId)
            .Include(p => p.Session)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueByProfessionalAsync(uint professionalId)
    {
        return await _dbContext.Payments
            .Where(p => p.Session.ProfessionalId == professionalId && p.Status == "completed")
            .SumAsync(p => p.TotalAmount);
    }
}
