using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
    // Operaciones específicas de negocio para User
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    Task<User?> GetUserWithProfilesAsync(uint userId);
    Task<IEnumerable<User>> GetProfessionalsAsync();
}
