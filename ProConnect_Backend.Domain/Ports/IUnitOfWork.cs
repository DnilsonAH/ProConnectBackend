
using ProConnect_Backend.Domain.Ports.IRepositories;

namespace ProConnect_Backend.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    // Repositorios
    IUserRepository UserRepository { get; }
    IJwtBlacklistRepository JwtBlacklistRepository { get; }
    ISessionRepository SessionRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IProfessionalProfileRepository ProfessionalProfileRepository { get; }
    IProfessionRepository ProfessionRepository { get; }
    IProfessionCategoryRepository ProfessionCategoryRepository { get; }
    ISpecializationRepository SpecializationRepository { get; }
    IVerificationRepository VerificationRepository { get; }
    IVerificationDocumentRepository VerificationDocumentRepository { get; }
    IWeeklyAvailabilityRepository WeeklyAvailabilityRepository { get; }
    IScheduledRepository ScheduledRepository { get; }
    IProfileSpecializationRepository ProfileSpecializationRepository { get; }
    
    // Commit de cambios
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}