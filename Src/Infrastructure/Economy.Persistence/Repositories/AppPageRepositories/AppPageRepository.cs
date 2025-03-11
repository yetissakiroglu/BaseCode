using Economy.Application.Repositories.AppPageRepositories;
using Economy.Domain.Entites.EntityAppPages;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppPageRepositories
{
    public class AppPageRepository(AppDbContext context) : EfEntityRepositoryBase<AppPage, int>(context), IAppPageRepository
    {



    }
  
}
