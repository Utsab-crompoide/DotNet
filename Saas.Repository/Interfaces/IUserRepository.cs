using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saas.Entities;

namespace Saas.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUserIdAsync(Guid id);

    }
}