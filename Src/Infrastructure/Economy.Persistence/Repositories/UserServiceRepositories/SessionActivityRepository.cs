using Economy.Application.Repositories.UserServiceRepositories;
using Economy.Domain.Entites;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.UserServiceRepositories
{
    public class SessionActivityRepository(DefaultDbContext context) : EfEntityRepositoryBase<SessionActivity, string>(context), ISessionActivityRepository
    {
 


	}
}
