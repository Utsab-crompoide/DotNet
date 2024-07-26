using Entities;

namespace Saas.Repository.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task<List<Organization>> GetOrganizationsByUserIdAsync(Guid id);
        Task<bool> CheckOrganizationByUserId(Guid userId, Guid id);
    }
}