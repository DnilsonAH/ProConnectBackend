using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(uint userId);
    Task RemoveUserRoleAsync(uint userId, uint roleId);
}
