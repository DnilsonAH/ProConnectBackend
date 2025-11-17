using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IProfessionalProfileRepository : IGenericRepository<ProfessionalProfile>
{
    // Operaciones específicas de negocio para ProfessionalProfile
    Task<ProfessionalProfile?> GetByUserIdAsync(uint userId);
    Task<IEnumerable<ProfessionalProfile>> GetProfilesByProfessionAsync(uint professionId);
    Task<ProfessionalProfile?> GetProfileWithDetailsAsync(uint profileId);
    Task<IEnumerable<ProfessionalProfile>> GetVerifiedProfilesAsync();
    Task<IEnumerable<ProfessionalProfile>> GetProfilesByAvailabilityAsync(DayOfWeek day);
}
