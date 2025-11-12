
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;

namespace ProConnect_Backend.Infrastructure.Adapters;

public class UnitOfWork: IUnitOfWork
{
    private readonly ProConnectDbContext _dbContext;
    
    // Propiedades públicas de solo lectura
    public IUserRepository UserRepository { get; }
    public IJwtBlacklistRepository JwtBlacklistRepository { get; }
    public ISessionRepository SessionRepository { get; }
    public IPaymentRepository PaymentRepository { get; }
    public IReviewRepository ReviewRepository { get; }
    public IProfessionalProfileRepository ProfessionalProfileRepository { get; }
    public IProfessionRepository ProfessionRepository { get; }
    public IProfessionCategoryRepository ProfessionCategoryRepository { get; }
    public ISpecializationRepository SpecializationRepository { get; }
    public IVerificationRepository VerificationRepository { get; }
    public IVerificationDocumentRepository VerificationDocumentRepository { get; }
    public IWeeklyAvailabilityRepository WeeklyAvailabilityRepository { get; }
    public IScheduledRepository ScheduledRepository { get; }

    public UnitOfWork(
        ProConnectDbContext dbContext,
        IUserRepository userRepository,
        IJwtBlacklistRepository jwtBlacklistRepository,
        ISessionRepository sessionRepository,
        IPaymentRepository paymentRepository,
        IReviewRepository reviewRepository,
        IProfessionalProfileRepository professionalProfileRepository,
        IProfessionRepository professionRepository,
        IProfessionCategoryRepository professionCategoryRepository,
        ISpecializationRepository specializationRepository,
        IVerificationRepository verificationRepository,
        IVerificationDocumentRepository verificationDocumentRepository,
        IWeeklyAvailabilityRepository weeklyAvailabilityRepository,
        IScheduledRepository scheduledRepository)
    { 
        _dbContext = dbContext;
        UserRepository = userRepository;
        JwtBlacklistRepository = jwtBlacklistRepository;
        SessionRepository = sessionRepository;
        PaymentRepository = paymentRepository;
        ReviewRepository = reviewRepository;
        ProfessionalProfileRepository = professionalProfileRepository;
        ProfessionRepository = professionRepository;
        ProfessionCategoryRepository = professionCategoryRepository;
        SpecializationRepository = specializationRepository;
        VerificationRepository = verificationRepository;
        VerificationDocumentRepository = verificationDocumentRepository;
        WeeklyAvailabilityRepository = weeklyAvailabilityRepository;
        ScheduledRepository = scheduledRepository;
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