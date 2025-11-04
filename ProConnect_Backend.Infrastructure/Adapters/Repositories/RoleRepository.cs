using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(ProConnectDbContext context) : base(context)
    {
    }
    
    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _dbContext.Set<Role>()
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
    }
}
