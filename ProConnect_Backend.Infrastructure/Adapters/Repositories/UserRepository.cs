using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(ProConnectDbContext context) : base(context)
    {
    }
    //Agregar los metodos específicos
}