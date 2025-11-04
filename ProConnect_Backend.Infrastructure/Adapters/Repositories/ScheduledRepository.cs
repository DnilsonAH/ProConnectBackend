using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ScheduledRepository: GenericRepository<Scheduled>, IScheduledRepository
{
    public ScheduledRepository(ProConnectDbContext context) : base(context)
    {
    }
}

