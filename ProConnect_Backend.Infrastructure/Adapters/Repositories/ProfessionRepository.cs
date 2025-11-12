using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ProfessionRepository : GenericRepository<Profession>, IProfessionRepository
{
    public ProfessionRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Profession?> GetByNameAsync(string professionName)
    {
        return await _dbContext.Professions
            .FirstOrDefaultAsync(p => p.ProfessionName == professionName);
    }

    public async Task<IEnumerable<Profession>> GetProfessionsByCategoryAsync(uint categoryId)
    {
        return await _dbContext.Professions
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Profession?> GetProfessionWithCategoryAsync(uint professionId)
    {
        return await _dbContext.Professions
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProfessionId == professionId);
    }
}
