using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface ISpecializationRepository : IGenericRepository<Specialization>
{
    // Operaciones específicas de negocio para Specialization
    Task<IEnumerable<Specialization>> GetSpecializationsByProfileIdAsync(uint profileId);
    Task<Specialization?> GetByNameAsync(string specializationName);
    Task<bool> ExistsByNameAsync(string specializationName);
}
