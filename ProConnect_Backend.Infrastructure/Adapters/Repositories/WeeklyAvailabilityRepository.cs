using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class WeeklyAvailabilityRepository : GenericRepository<WeeklyAvailability>, IWeeklyAvailabilityRepository
{
    public WeeklyAvailabilityRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<WeeklyAvailability>> GetAvailabilityByProfessionalIdAsync(uint professionalId)
    {
        return await _dbContext.WeeklyAvailabilities
            .Where(w => w.ProfessionalId == professionalId)
            .OrderBy(w => w.WeekDay)
            .ThenBy(w => w.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<WeeklyAvailability>> GetAvailabilityByDayAsync(string dayOfWeek)
    {
        return await _dbContext.WeeklyAvailabilities
            .Where(w => w.WeekDay == dayOfWeek && w.IsAvailable == true)
            .Include(w => w.Professional)
            .ToListAsync();
    }

    public async Task<IEnumerable<WeeklyAvailability>> GetAvailableSlotsAsync(uint professionalId, string dayOfWeek)
    {
        return await _dbContext.WeeklyAvailabilities
            .Where(w => w.ProfessionalId == professionalId 
                     && w.WeekDay == dayOfWeek 
                     && w.IsAvailable == true)
            .OrderBy(w => w.StartTime)
            .ToListAsync();
    }

    public async Task<bool> IsAvailableAsync(uint professionalId, string dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        return await _dbContext.WeeklyAvailabilities
            .AnyAsync(w => w.ProfessionalId == professionalId 
                        && w.WeekDay == dayOfWeek 
                        && w.StartTime <= startTime 
                        && w.EndTime >= endTime 
                        && w.IsAvailable == true);
    }
}
