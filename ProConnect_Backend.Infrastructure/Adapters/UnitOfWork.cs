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



    // El constructor ahora recibe todo lo necesario
    public UnitOfWork(
        ProConnectDbContext dbContext)
        //IAvailabilityRepository availabilityRepository,

    { _dbContext = dbContext;
        
        // Simplemente asigna las instancias recibidas
        //AvailabilityRepository = availabilityRepository;

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