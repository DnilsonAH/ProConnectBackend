using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class WeeklyAvailabilityRepository: GenericRepository<WeeklyAvailability>, IWeeklyAvailabilityRepository
{
    public WeeklyAvailabilityRepository(ProConnectDbContext context) : base(context)
    {
    }
}
