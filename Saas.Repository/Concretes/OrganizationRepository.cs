using Entities;
using Microsoft.EntityFrameworkCore;
using Saas.Data;
using Saas.Repository.Interfaces;

namespace Saas.Repository.Concretes;
public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(ApplicationDbContext context) : base(context)
    {
    }
    //Todo GetUser Organization from User Organization repository
     public async Task<List<Organization>> GetOrganizationsByUserIdAsync(Guid id)
    {
        return await GetAll().Where(u => u.CreatedBy == id).ToListAsync();
    }
    public async Task<bool> CheckOrganizationByUserId(Guid userId, Guid id)
    {
        return await GetAll().AnyAsync(s => s.CreatedBy == userId && s.Id == id);
    }
}
