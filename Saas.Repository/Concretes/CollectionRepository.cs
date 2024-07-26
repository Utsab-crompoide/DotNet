using Entities;
using Microsoft.EntityFrameworkCore;
using Saas.Repository.Interfaces;
using Saas.Data;

namespace Saas.Repository.Concretes;
public class CollectionRepository : Repository<Collection>, ICollectionRepository
{
    public CollectionRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<List<Collection>> GetCollectionsByUserIdAsync(Guid id)
    {
        return await GetAll().Where(u => u.CreatedBy == id).ToListAsync();
    }
    public async Task<bool> CheckCollectionByUserId(Guid userId, Guid id)
    {
        return await GetAll().AnyAsync(s => s.CreatedBy == userId && s.Id == id);
    }
    public async Task<bool> CheckCollectionExistsByUserId(Guid userId, string name)
    {
        return await GetAll().AnyAsync(s => s.CreatedBy == userId && s.Name.Trim() == name.Trim());
    }
    public async Task<List<Collection>> GetCollectionsByIdsAsync(List<Guid> collectionIds)
    {
        return await GetAll().Where(f => collectionIds.Contains(f.Id)).ToListAsync();
    }
}