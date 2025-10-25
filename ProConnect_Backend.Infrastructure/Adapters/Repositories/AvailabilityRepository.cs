using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class AvailabilityRepository: GenericRepository<Availability>, IAvailabilityRepository
{
    public AvailabilityRepository(ProConnectDbContext context) : base(context)
    {
    }
    //Agregar los metodos específicos
    
}