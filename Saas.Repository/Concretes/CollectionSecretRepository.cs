using Entities;
using Microsoft.EntityFrameworkCore;
using Saas.Repository.Interfaces;
using Saas.Data;
using Saas.Entities;

namespace Saas.Repository.Concretes;
public class CollectionSecretRepository : Repository<CollectionSecret>, ICollectionSecretRepository
{
    public CollectionSecretRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<List<CollectionSecret>> GetCollectionsBySecretIdAsync(Guid secretId)
    {
        return await GetAll().Where(s => s.SecretId == secretId).ToListAsync();
    }
}