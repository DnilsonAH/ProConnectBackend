using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IWeeklyAvailabilityRepository : IGenericRepository<WeeklyAvailability>
{
    // Operaciones específicas de negocio para WeeklyAvailability
    Task<IEnumerable<WeeklyAvailability>> GetAvailabilityByProfessionalIdAsync(uint professionalId);
    Task<IEnumerable<WeeklyAvailability>> GetAvailabilityByDayAsync(string dayOfWeek);
    Task<IEnumerable<WeeklyAvailability>> GetAvailableSlotsAsync(uint professionalId, string dayOfWeek);
    Task<bool> IsAvailableAsync(uint professionalId, string dayOfWeek, TimeOnly startTime, TimeOnly endTime);
}
