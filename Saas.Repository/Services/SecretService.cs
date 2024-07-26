using Saas.Data;
using Saas.Entities;
using Saas.Entities.Dtos;
using Saas.Repository.Interfaces;

namespace Saas.Repository.Services
{
    public class SecretService(
        ISecretRepository _secretRepository,
    ICollectionSecretRepository _collectionSecretRepository,
    ApplicationDbContext _dbContext
    ) : ISecretService
    {
        public async Task<Result> AddSecretWithCollections(SecretWithCollection secretWithCollection)
        {

            var secret = secretWithCollection.Secret;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _secretRepository.AddAsync(secret);
                var secretCollections = secretWithCollection.CollectionIds?.Select(collectionIds => new CollectionSecret
                {
                    SecretId = secret.Id,
                    CollectionId = collectionIds
                }).ToList();
                if (secretCollections is not null)
                {
                    await _collectionSecretRepository.AddRangeAsync(secretCollections);
                }
                await transaction.CommitAsync();
            }

            return new Result(true, "Secret added successfully");
        }

        public async Task<Result> UpdateSecretWithCollections(SecretWithCollection secretWithCollection)
        {
            var secret = secretWithCollection.Secret;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                await _secretRepository.UpdateAsync(secret);
                var secretCollections = secretWithCollection.CollectionIds?.Select(collectionIds => new CollectionSecret
                {
                    SecretId = secret.Id,
                    CollectionId = collectionIds
                }).ToList();

                if (secretWithCollection.CollectionIds is not null)
                {
                    var existingSecretCollections = await _collectionSecretRepository.GetCollectionsBySecretIdAsync(secretWithCollection.Secret.Id);
                    if (existingSecretCollections is not null)
                    {
                        await _collectionSecretRepository.DeleteRangeAsync(existingSecretCollections);
                    }
                    if (secretCollections is not null)
                    {
                        await _collectionSecretRepository.AddRangeAsync(secretCollections);
                    }
                    await transaction.CommitAsync();
                }
            }

            return new Result(true, "Secret updated successfully");
        }
    }
}