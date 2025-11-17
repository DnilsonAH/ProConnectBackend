using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ProfileSpecializationRepository : GenericRepository<ProfileSpecialization>, IProfileSpecializationRepository
{
    public ProfileSpecializationRepository(ProConnectDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProfileSpecialization>> GetByProfileIdAsync(uint profileId)
    {
        return await _dbContext.ProfileSpecializations
            .Where(ps => ps.ProfileId == profileId)
            .Include(ps => ps.Specialization)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProfileSpecialization>> GetBySpecializationIdAsync(uint specializationId)
    {
        return await _dbContext.ProfileSpecializations
            .Where(ps => ps.SpecializationId == specializationId)
            .Include(ps => ps.Profile)
            .ToListAsync();
    }

    public async Task<ProfileSpecialization?> GetByProfileAndSpecializationAsync(uint profileId, uint specializationId)
    {
        return await _dbContext.ProfileSpecializations
            .FirstOrDefaultAsync(ps => ps.ProfileId == profileId && ps.SpecializationId == specializationId);
    }

    public async Task<bool> ExistsAsync(uint profileId, uint specializationId)
    {
        return await _dbContext.ProfileSpecializations
            .AnyAsync(ps => ps.ProfileId == profileId && ps.SpecializationId == specializationId);
    }
}
