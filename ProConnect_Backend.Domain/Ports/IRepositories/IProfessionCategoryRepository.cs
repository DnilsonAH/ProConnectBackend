using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IProfessionCategoryRepository : IGenericRepository<ProfessionCategory>
{
    // Operaciones específicas de negocio para ProfessionCategory
    Task<ProfessionCategory?> GetByNameAsync(string categoryName);
    Task<IEnumerable<ProfessionCategory>> GetCategoriesWithProfessionsAsync();
}
