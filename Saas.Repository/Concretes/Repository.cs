using Data;
using Microsoft.EntityFrameworkCore;
using Saas.Data;
using Saas.Repository.Interfaces;

namespace Saas.Repository.Concretes;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    public readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsNoTracking().AsQueryable();
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteRangeAsync(List<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }
}
