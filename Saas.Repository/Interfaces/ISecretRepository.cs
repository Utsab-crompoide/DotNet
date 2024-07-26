using Entities;
using Saas.Entities.Dtos;
using Saas.Entities.enums;

namespace Saas.Repository.Interfaces
{
    public interface ISecretRepository : IRepository<Secret>
    {
       Task<PaginatedResult<Secret>> GetSecretByUserIdAsync(Guid id, int currentPage, int pageSize);
       Task<List<Secret>> GetSecretByUserIdAsync(Guid id);
       Task<PaginatedResult<Secret>> GetSecretByUserIdAndSecretTypeAsync(Guid id, SecretType secretType, int currentPage, int pageSize);
       Task<List<Secret>> GetSecretByUserIdAndSecretTypeAsync(Guid id, SecretType secretType);
       Task<bool> CheckSecretByUserId(Guid userId, Guid id);
       Task<Secret?> GetSecretByIdIncludeCollectionAsync(Guid id);
       Task<List<Secret>> GetSecretByUserIdAndCollectionAsync(Guid id, Guid collectionId);
    }
}