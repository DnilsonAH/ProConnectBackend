using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IUserRepository: IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetUserWithRolesAsync(uint userId);
}