using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(ProConnectDbContext context) : base(context)
    {
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Set<User>().SingleOrDefaultAsync(u => u.Email == email);
    }
    
}