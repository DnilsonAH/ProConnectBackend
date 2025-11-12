
using ProConnect_Backend.Domain.Ports.IRepositories;

namespace ProConnect_Backend.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    // TODO: Descomentar después del scaffolding cuando las interfaces de repositorios estén creadas
    
    // Repositorios
    // IPaymentRepository PaymentRepository { get; }
    // IReviewRepository ReviewRepository { get; }
    // IScheduledRepository ScheduledRepository { get; }
    // ISessionRepository SessionRepository { get; }
    // ISpecialtyRepository SpecialtyRepository { get; }
    // IUserRepository UserRepository { get; }
    // IWeeklyAvailabilityRepository WeeklyAvailabilityRepository { get; }
    // IVerificationDocumentRepository VerificationDocumentRepository { get; }
    // IVerificationRepository VerificationRepository { get; }
    // IRevokedTokenRepository RevokedTokenRepository { get; }
    
    // Commmit de cambios
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}