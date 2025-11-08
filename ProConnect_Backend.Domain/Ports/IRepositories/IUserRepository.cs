using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}