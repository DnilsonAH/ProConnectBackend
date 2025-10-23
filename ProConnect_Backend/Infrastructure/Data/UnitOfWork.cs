using ProConnect_Backend.Core.Repositories.Interfaces;

namespace ProConnect_Backend.Infrastructure.Data;

public class UnitOfWork: IUnitOfWork
{
    private readonly ProConnectDbContext _dbContext;
    
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

    public UnitOfWork(ProConnectDbContext dbContext, 
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
    {
        _dbContext = dbContext;
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
        VerificationDocumentRepository = VerificationDocumentRepository;
        VerificationRepository = verificationRepository;
        VideoCallRepository = videoCallRepository;
    }
    public async Task<int> CompleteAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}