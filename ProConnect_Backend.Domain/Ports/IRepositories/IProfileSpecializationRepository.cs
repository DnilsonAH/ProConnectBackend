using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IProfileSpecializationRepository : IGenericRepository<ProfileSpecialization>
{
    // Operaciones específicas de negocio para ProfileSpecialization
    Task<IEnumerable<ProfileSpecialization>> GetByProfileIdAsync(uint profileId);
    Task<IEnumerable<ProfileSpecialization>> GetBySpecializationIdAsync(uint specializationId);
    Task<ProfileSpecialization?> GetByProfileAndSpecializationAsync(uint profileId, uint specializationId);
    Task<bool> ExistsAsync(uint profileId, uint specializationId);
}
