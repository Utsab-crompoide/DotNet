using Entities;

namespace Saas.Repository.Interfaces
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        Task<List<Collection>> GetCollectionsByUserIdAsync(Guid id);
        Task<bool> CheckCollectionByUserId(Guid userId, Guid id);
        Task<bool> CheckCollectionExistsByUserId(Guid userId, string name);
        Task<List<Collection>> GetCollectionsByIdsAsync(List<Guid> collectionIds);

    }
}