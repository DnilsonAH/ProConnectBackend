using ProConnect_Backend.Domain.Interfaces;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;

namespace ProConnect_Backend.Infrastructure.Adapters;

public class UnitOfWork: IUnitOfWork
{
    private readonly ProConnectDbContext _dbContext;
    
    // Las propiedades ahora son públicas y de solo lectura
    //public IAvailabilityRepository AvailabilityRepository { get; }
    
    public IPaymentRepository PaymentRepository { get; }
    public IReviewRepository ReviewRepository { get; }
    public IScheduledRepository ScheduledRepository { get; }
    public ISessionRepository SessionRepository { get; }
    public ISpecialtyRepository SpecialtyRepository { get; }
    public IUserRepository UserRepository { get; }
    public IVerificationDocumentRepository VerificationDocumentRepository { get; }
    public IVerificationRepository VerificationRepository { get; } 
    public IWeeklyAvailabilityRepository WeeklyAvailabilityRepository { get; }



    // El constructor ahora recibe todo lo necesario
    public UnitOfWork(
        ProConnectDbContext dbContext,
        IPaymentRepository paymentRepository,
        IReviewRepository reviewRepository,
        IScheduledRepository scheduledRepository,
        ISessionRepository sessionRepository,
        ISpecialtyRepository specialtyRepository,
        IUserRepository userRepository,
        IVerificationDocumentRepository verificationDocumentRepository,
        IVerificationRepository verificationRepository,
        IWeeklyAvailabilityRepository weeklyAvailabilityRepository)

    { _dbContext = dbContext;
        PaymentRepository = paymentRepository;
        ReviewRepository = reviewRepository;
        ScheduledRepository = scheduledRepository;
        SessionRepository = sessionRepository;
        SpecialtyRepository = specialtyRepository;
        UserRepository = userRepository;
        VerificationDocumentRepository = verificationDocumentRepository;
        VerificationRepository = verificationRepository;
        WeeklyAvailabilityRepository = weeklyAvailabilityRepository;

    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}