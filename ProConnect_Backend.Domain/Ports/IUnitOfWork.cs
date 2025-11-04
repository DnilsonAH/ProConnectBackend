using ProConnect_Backend.Domain.Ports.IRepositories;

namespace ProConnect_Backend.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    // Repositorios
    //IAvailabilityRepository AvailabilityRepository { get; }

    
    // Commmit de cambios
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}