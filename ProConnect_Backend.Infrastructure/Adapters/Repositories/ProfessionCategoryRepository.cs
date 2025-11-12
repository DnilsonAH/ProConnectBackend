using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ProfessionCategoryRepository : GenericRepository<ProfessionCategory>, IProfessionCategoryRepository
{
    public ProfessionCategoryRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ProfessionCategory?> GetByNameAsync(string categoryName)
    {
        return await _dbContext.ProfessionCategories
            .FirstOrDefaultAsync(c => c.CategoryName == categoryName);
    }

    public async Task<IEnumerable<ProfessionCategory>> GetCategoriesWithProfessionsAsync()
    {
        return await _dbContext.ProfessionCategories
            .Include(c => c.Professions)
            .ToListAsync();
    }
}
