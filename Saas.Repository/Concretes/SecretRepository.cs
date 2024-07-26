using Data;
using Entities;
using Microsoft.EntityFrameworkCore;
using Saas.Data;
using Saas.Entities.Dtos;
using Saas.Entities.enums;
using Saas.Repository.Interfaces;

namespace Saas.Repository.Concretes;
public class SecretRepository : Repository<Secret>, ISecretRepository
{
    public SecretRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<Secret?> GetSecretByIdIncludeCollectionAsync(Guid id)
    {
        return await GetAll().Where(x => x.Id == id).Include(x => x.CollectionSecrets).ThenInclude(c => c.Collection).FirstOrDefaultAsync();
    }
    public async Task<PaginatedResult<Secret>> GetSecretByUserIdAsync(Guid id, int currentPage, int pageSize)
    {
        var query = GetAll().Where(u => u.CreatedBy == id);
        var totalItems = await query.CountAsync();
        var data = await query.Include(x=>x.CollectionSecrets).ThenInclude(c=>c.Collection).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedResult<Secret>(data, totalItems, currentPage, pageSize);

    }
    public async Task<List<Secret>> GetSecretByUserIdAsync(Guid id)
    {
        var query = GetAll().Where(u => u.CreatedBy == id);
        return await query.Include(x=>x.CollectionSecrets).ThenInclude(c=>c.Collection).ToListAsync();
    }
    public async Task<PaginatedResult<Secret>> GetSecretByUserIdAndSecretTypeAsync(Guid id, SecretType secretType, int currentPage, int pageSize)
    {
        var query = GetAll().Where(u => u.CreatedBy == id && u.Type == secretType);
        var totalItems = await query.CountAsync();
        var data = await query.Include(x=>x.CollectionSecrets).ThenInclude(c=>c.Collection).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedResult<Secret>(data, totalItems, currentPage, pageSize);
    }
    public async Task<List<Secret>> GetSecretByUserIdAndSecretTypeAsync(Guid id, SecretType secretType)
    {
        var query = GetAll().Where(u => u.CreatedBy == id && u.Type == secretType);
       return await query.Include(x=>x.CollectionSecrets).ThenInclude(c=>c.Collection).ToListAsync();
    }
    public async Task<List<Secret>> GetSecretByUserIdAndCollectionAsync(Guid id, Guid collectionId)
    {
        var query = GetAll().Where(u => u.CreatedBy == id && u.CollectionSecrets.Any(c=>c.CollectionId==collectionId));
       return await query.ToListAsync();
    }
    public async Task<bool> CheckSecretByUserId(Guid userId, Guid id)
    {
        return await GetAll().AnyAsync(s => s.CreatedBy == userId && s.Id == id);
    }
}