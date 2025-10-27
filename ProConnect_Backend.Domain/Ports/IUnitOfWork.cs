using ProConnect_Backend.Domain.Ports.IRepositories;

namespace ProConnect_Backend.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    // Repositorios
    IAvailabilityRepository AvailabilityRepository { get; }
    IConsultationRepository ConsultationRepository { get; }
    IDistributionRepository DistributionRepository { get; }
    INotificationRepository NotificationRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IProfessionalPaymentInfoRepository ProfessionalPaymentInfoRepository { get; }
    IProfessionalProfileRepository ProfessionalProfileRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IRoleRepository RoleRepository { get; }
    ISpecialtyRepository SpecialtyRepository { get; }
    IUserRepository UserRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    IVerificationDocumentRepository VerificationDocumentRepository { get; }
    IVerificationRepository VerificationRepository { get; }
    IVideoCallRepository VideoCallRepository { get; }
    
    // Commmit de cambios
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}