using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Interfaces;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ProConnectDbContext _context;

    public UserRepository(ProConnectDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}