namespace ProConnect_Backend.Domain.Ports.IRepositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}