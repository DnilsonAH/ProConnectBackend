using ProConnect_Backend.Domain.Interfaces;
using ProConnect_Backend.Domain.Ports.IRepositories;

namespace ProConnect_Backend.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    // Repositorios
    IPaymentRepository PaymentRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IScheduledRepository ScheduledRepository { get; }
    ISessionRepository SessionRepository { get; }
    ISpecialtyRepository SpecialtyRepository { get; }
    IUserRepository UserRepository { get; }
    IWeeklyAvailabilityRepository WeeklyAvailabilityRepository { get; }
    IVerificationDocumentRepository VerificationDocumentRepository { get; }
    IVerificationRepository VerificationRepository { get; }
    
     
    
    // Commmit de cambios
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}