using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Ports.IRepositories;
// TODO: Descomentar después del scaffolding
// using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity : class
{
    // TODO: Descomentar después del scaffolding cuando ProConnectDbContext esté generado
    // protected readonly ProConnectDbContext _dbContext;
    // public GenericRepository(ProConnectDbContext dbContext)
    // {
    //     _dbContext = dbContext;
    // }
    // TODO: Descomentar después del scaffolding
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        // return await _dbContext.Set<TEntity>().ToListAsync();
        return await Task.FromResult(Enumerable.Empty<TEntity>());
    }
    
    public async Task<TEntity?> GetByIdAsync(uint id)
    {
        // return await _dbContext.Set<TEntity>().FindAsync(id);
        return await Task.FromResult<TEntity?>(null);
    }

    public async Task AddAsync(TEntity entity)
    {
        // await _dbContext.Set<TEntity>().AddAsync(entity);
        // await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
    }
    
    public void Update(TEntity entity)
    {
        // _dbContext.Set<TEntity>().Update(entity);
        // _dbContext.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        // _dbContext.Set<TEntity>().Remove(entity);
        // _dbContext.SaveChanges();
    }
}