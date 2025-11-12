using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ProConnectDbContext _dbContext;
    public GenericRepository(ProConnectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }
    public async Task<TEntity?> GetByIdAsync(uint id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        _dbContext.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        _dbContext.SaveChanges();
    }
}