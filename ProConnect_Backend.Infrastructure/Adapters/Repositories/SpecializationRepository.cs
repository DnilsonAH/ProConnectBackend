using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Specialization>> GetSpecializationsByProfileIdAsync(uint profileId)
    {
        return await _dbContext.Specializations
            .Where(s => s.ProfileSpecializations.Any(ps => ps.ProfileId == profileId))
            .ToListAsync();
    }

    public async Task<Specialization?> GetByNameAsync(string specializationName)
    {
        return await _dbContext.Specializations
            .FirstOrDefaultAsync(s => s.SpecializationName == specializationName);
    }

    public async Task<bool> ExistsByNameAsync(string specializationName)
    {
        return await _dbContext.Specializations
            .AnyAsync(s => s.SpecializationName == specializationName);
    }
}
