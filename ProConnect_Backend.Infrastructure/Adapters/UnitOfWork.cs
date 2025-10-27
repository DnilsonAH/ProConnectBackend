using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;

namespace ProConnect_Backend.Infrastructure.Adapters;

public class UnitOfWork: IUnitOfWork
{
    private readonly ProConnectDbContext _dbContext;
    
    private IAvailabilityRepository? _availabilityRepository;
    private IConsultationRepository? _consultationRepository;
    private IDistributionRepository? _distributionRepository;
    private INotificationRepository? _notificationRepository;
    private IPaymentRepository? _paymentRepository;
    private IProfessionalPaymentInfoRepository? _professionalPaymentInfoRepository;
    private IProfessionalProfileRepository? _professionalProfileRepository;
    private IReviewRepository? _reviewRepository;
    private IRoleRepository? _roleRepository;
    private ISpecialtyRepository? _specialtyRepository;
    private IUserRepository? _userRepository;
    private IUserRoleRepository? _userRoleRepository;
    private IVerificationDocumentRepository? _verificationDocumentRepository;
    private IVerificationRepository? _verificationRepository;
    private IVideoCallRepository? _videoCallRepository;


    public UnitOfWork(ProConnectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IAvailabilityRepository AvailabilityRepository =>
        _availabilityRepository ??= new AvailabilityRepository(_dbContext);

    public IConsultationRepository ConsultationRepository =>
        _consultationRepository ??= new ConsultationRepository(_dbContext);
    public IDistributionRepository DistributionRepository =>
        _distributionRepository ??= new DistributionRepository(_dbContext);
    public INotificationRepository NotificationRepository =>
        _notificationRepository ??= new NotificationRepository(_dbContext);
    public IPaymentRepository PaymentRepository =>
        _paymentRepository ??= new PaymentRepository(_dbContext);
    public IProfessionalPaymentInfoRepository ProfessionalPaymentInfoRepository =>
        _professionalPaymentInfoRepository ??= new ProfessionalPaymentInfoRepository(_dbContext);
    public IProfessionalProfileRepository ProfessionalProfileRepository =>
        _professionalProfileRepository ??= new ProfessionalProfileRepository(_dbContext);
    public IReviewRepository ReviewRepository =>
        _reviewRepository ??= new ReviewRepository(_dbContext);
    public IRoleRepository RoleRepository =>
        _roleRepository ??= new RoleRepository(_dbContext);
    public ISpecialtyRepository SpecialtyRepository =>
        _specialtyRepository ??= new SpecialtyRepository(_dbContext);
    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(_dbContext);
    public IUserRoleRepository UserRoleRepository =>
        _userRoleRepository ??= new UserRoleRepository(_dbContext);
    public IVerificationDocumentRepository VerificationDocumentRepository =>
        _verificationDocumentRepository ??= new VerificationDocumentRepository(_dbContext);
    public IVerificationRepository VerificationRepository =>
        _verificationRepository ??= new VerificationRepository(_dbContext);
    public IVideoCallRepository VideoCallRepository =>
        _videoCallRepository ??= new VideoCallRepository(_dbContext);
    
    
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