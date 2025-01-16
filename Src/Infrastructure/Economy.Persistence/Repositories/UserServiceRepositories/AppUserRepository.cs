using Economy.Application.Repositories.UserServiceRepositoriesa;
using Economy.Domain.Entites.Identities;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.UserServiceRepositories
{
    public class AppUserRepository(AppDbContext context) : EfEntityRepositoryBase<AppUser,string>(context), IAppUserRepository
    {




    }
}