namespace ProConnect_Backend.Core.Services.interfaces;

public interface IGenericService<Tdto> where Tdto : class
{
    Task<IEnumerable<Tdto>> GetAllAsync();
    Task<Tdto?> GetByIdAsync(int id);
    Task AddAsync(Tdto dto);
    Task UpdateAsync(Tdto dto);
    Task DeleteAsync(int id);
}