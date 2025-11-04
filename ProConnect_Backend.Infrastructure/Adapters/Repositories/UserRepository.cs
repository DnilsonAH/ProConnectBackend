/*using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

/*
public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(ProConnectDbContext context) : base(context)
    {
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Set<User>()
            .AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserWithRolesAsync(uint userId)
    {
        return await _dbContext.Set<User>()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}#1#*/