using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saas.Entities.Dtos;

namespace Saas.Repository.Interfaces
{
    public interface ISecretService
    {
        Task<Result> AddSecretWithCollections(SecretWithCollection secretWithCollection);
        Task<Result> UpdateSecretWithCollections(SecretWithCollection secretWithCollection);
    }
}