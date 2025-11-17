using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
    {
        return await _dbContext.Users
            .Where(u => u.Role == role)
            .ToListAsync();
    }

    public async Task<User?> GetUserWithProfilesAsync(uint userId)
    {
        return await _dbContext.Users
            .Include(u => u.ProfessionalProfile)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<IEnumerable<User>> GetProfessionalsAsync()
    {
        return await _dbContext.Users
            .Where(u => u.Role == "professional")
            .Include(u => u.ProfessionalProfile)
            .ToListAsync();
    }
}
