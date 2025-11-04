using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class SessionRepository: GenericRepository<Session>, ISessionRepository
{
    public SessionRepository(ProConnectDbContext context) : base(context)
    {
    }
}

