using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;

namespace ProConnect_Backend.Infrastructure.Adapters;

public class UnitOfWork: IUnitOfWork
{
    private readonly ProConnectDbContext _dbContext;
    
    // Las propiedades ahora son públicas y de solo lectura
    public IAvailabilityRepository AvailabilityRepository { get; }
    public IConsultationRepository ConsultationRepository { get; }
    public IDistributionRepository DistributionRepository { get; }
    public INotificationRepository NotificationRepository { get; }
    public IPaymentRepository PaymentRepository { get; }
    public IProfessionalPaymentInfoRepository ProfessionalPaymentInfoRepository { get; }
    public IProfessionalProfileRepository ProfessionalProfileRepository { get; }
    public IReviewRepository ReviewRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public ISpecialtyRepository SpecialtyRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserRoleRepository UserRoleRepository { get; }
    public IVerificationDocumentRepository VerificationDocumentRepository { get; }
    public IVerificationRepository VerificationRepository { get; }
    public IVideoCallRepository VideoCallRepository { get; }


    // El constructor ahora recibe todo lo necesario
    public UnitOfWork(
        ProConnectDbContext dbContext,
        IAvailabilityRepository availabilityRepository,
        IConsultationRepository consultationRepository,
        IDistributionRepository distributionRepository,
        INotificationRepository notificationRepository,
        IPaymentRepository paymentRepository,
        IProfessionalPaymentInfoRepository professionalPaymentInfoRepository,
        IProfessionalProfileRepository professionalProfileRepository,
        IReviewRepository reviewRepository,
        IRoleRepository roleRepository,
        ISpecialtyRepository specialtyRepository,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IVerificationDocumentRepository verificationDocumentRepository,
        IVerificationRepository verificationRepository,
        IVideoCallRepository videoCallRepository)
    { _dbContext = dbContext;
        
        // Simplemente asigna las instancias recibidas
        AvailabilityRepository = availabilityRepository;
        ConsultationRepository = consultationRepository;
        DistributionRepository = distributionRepository;
        NotificationRepository = notificationRepository;
        PaymentRepository = paymentRepository;
        ProfessionalPaymentInfoRepository = professionalPaymentInfoRepository;
        ProfessionalProfileRepository = professionalProfileRepository;
        ReviewRepository = reviewRepository;
        RoleRepository = roleRepository;
        SpecialtyRepository = specialtyRepository;
        UserRepository = userRepository;
        UserRoleRepository = userRoleRepository;
        VerificationDocumentRepository = verificationDocumentRepository;
        VerificationRepository = verificationRepository;
        VideoCallRepository = videoCallRepository;
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