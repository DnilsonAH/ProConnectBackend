using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(ProConnectDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(uint userId)
    {
        return await _dbContext.Set<UserRole>()
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId)
            .ToListAsync();
    }

    public async Task RemoveUserRoleAsync(uint userId, uint roleId)
    {
        var userRole = await _dbContext.Set<UserRole>()
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        
        if (userRole != null)
        {
            _dbContext.Set<UserRole>().Remove(userRole);
            await _dbContext.SaveChangesAsync();
        }
    }
}
