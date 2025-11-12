using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IProfessionRepository : IGenericRepository<Profession>
{
    // Operaciones específicas de negocio para Profession
    Task<Profession?> GetByNameAsync(string professionName);
    Task<IEnumerable<Profession>> GetProfessionsByCategoryAsync(uint categoryId);
    Task<Profession?> GetProfessionWithCategoryAsync(uint professionId);
}
