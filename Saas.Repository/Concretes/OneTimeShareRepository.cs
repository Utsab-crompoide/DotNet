using Saas.Data;
using Saas.Entities;
using Saas.Repository.Interfaces;

namespace Saas.Repository.Concretes;
public class OneTimeShareRepository(ApplicationDbContext context) : Repository<OneTimeShare>(context), IOneTimeShareRepository
{
}