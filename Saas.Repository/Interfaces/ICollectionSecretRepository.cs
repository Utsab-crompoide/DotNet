using Entities;
using Saas.Entities;

namespace Saas.Repository.Interfaces
{
    public interface ICollectionSecretRepository : IRepository<CollectionSecret>
    {
        Task<List<CollectionSecret>> GetCollectionsBySecretIdAsync(Guid secretId);
    }
}