using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppMenuRepositories
{
    public class AppMenuRepository(DefaultDbContext context) : EfEntityRepositoryBase<AppMenu>(context), IAppMenuRepository
    {



    }

}