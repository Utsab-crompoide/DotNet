using Saas.Data;
using Microsoft.EntityFrameworkCore;
using Saas.Entities;
using Saas.Repository.Interfaces;

namespace Saas.Repository.Concretes;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByUserIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }
}