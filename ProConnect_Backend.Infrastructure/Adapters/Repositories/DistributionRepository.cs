using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class DistributionRepository : GenericRepository<Distribution>, IDistributionRepository
{
    public DistributionRepository(ProConnectDbContext context) : base(context)
    {
    }
    //Agregar los metodos específicos
}