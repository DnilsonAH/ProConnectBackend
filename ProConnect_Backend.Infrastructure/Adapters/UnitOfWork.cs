
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
// TODO: Descomentar después del scaffolding
// using ProConnect_Backend.Infrastructure.Data;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;

namespace ProConnect_Backend.Infrastructure.Adapters;

public class UnitOfWork: IUnitOfWork
{
    // TODO: Descomentar después del scaffolding
    // private readonly ProConnectDbContext _dbContext;
    
    // TODO: Descomentar después del scaffolding cuando las interfaces y repositorios estén creados
    
    // Las propiedades ahora son públicas y de solo lectura
    //public IAvailabilityRepository AvailabilityRepository { get; }
    
    // public IPaymentRepository PaymentRepository { get; }
    // public IReviewRepository ReviewRepository { get; }
    // public IScheduledRepository ScheduledRepository { get; }
    // public ISessionRepository SessionRepository { get; }
    // public ISpecialtyRepository SpecialtyRepository { get; }
    // public IUserRepository UserRepository { get; }
    // public IVerificationDocumentRepository VerificationDocumentRepository { get; }
    // public IVerificationRepository VerificationRepository { get; } 
    // public IWeeklyAvailabilityRepository WeeklyAvailabilityRepository { get; }
    // public IRevokedTokenRepository RevokedTokenRepository { get; }

    // El constructor ahora recibe todo lo necesario
    // TODO: Descomentar después del scaffolding
    public UnitOfWork()
    { 
        // _dbContext = dbContext;
        // PaymentRepository = paymentRepository;
        // ReviewRepository = reviewRepository;
        // ScheduledRepository = scheduledRepository;
        // SessionRepository = sessionRepository;
        // SpecialtyRepository = specialtyRepository;
        // UserRepository = userRepository;
        // VerificationDocumentRepository = verificationDocumentRepository;
        // VerificationRepository = verificationRepository;
        // WeeklyAvailabilityRepository = weeklyAvailabilityRepository;
        // RevokedTokenRepository = revokedTokenRepository;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Descomentar después del scaffolding
        // return await _dbContext.SaveChangesAsync(cancellationToken);
        return await Task.FromResult(0);
    }
    
    public void Dispose()
    {
        // TODO: Descomentar después del scaffolding
        // _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}