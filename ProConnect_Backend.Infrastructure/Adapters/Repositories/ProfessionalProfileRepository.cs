using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ProfessionalProfileRepository : GenericRepository<ProfessionalProfile>, IProfessionalProfileRepository
{
    public ProfessionalProfileRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ProfessionalProfile?> GetByUserIdAsync(uint userId)
    {
        return await _dbContext.ProfessionalProfiles
            .Include(p => p.Specialization)
            .Include(p => p)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<IEnumerable<ProfessionalProfile>> GetProfilesByProfessionAsync(uint professionId)
    {
        return await _dbContext.ProfessionalProfiles
            .Where(p => p.Specialization.ProfessionId == professionId)
            .Include(p => p.User)
            .Include(p => p)
            .ToListAsync();
    }

    public async Task<ProfessionalProfile?> GetProfileWithDetailsAsync(uint profileId)
    {
        return await _dbContext.ProfessionalProfiles
            .Include(p => p.User)
            .Include(p => p.Specialization)
            .Include(p => p)
            .FirstOrDefaultAsync(p => p.ProfileId == profileId);
    }

    public async Task<IEnumerable<ProfessionalProfile>> GetVerifiedProfilesAsync()
    {
        return await _dbContext.ProfessionalProfiles
            .Where(p => p.UserId > 0)
            .Include(p => p.User)
            .Include(p => p.Specialization)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProfessionalProfile>> GetProfilesByAvailabilityAsync(DayOfWeek day)
    {
        var dayString = day.ToString().ToLower();
        return await _dbContext.ProfessionalProfiles
            .Where(p => p.User.WeeklyAvailabilities.Any(wa => wa.WeekDay == dayString && wa.IsAvailable == true))
            .Include(p => p.User)
            .Include(p => p.Specialization)
            .ToListAsync();
    }
}
