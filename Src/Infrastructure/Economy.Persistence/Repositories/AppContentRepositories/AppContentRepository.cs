using Economy.Application.Repositories.AppContentRepositories;
using Economy.Domain.Entites.EntityAppContents.AppContents;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppContentRepositories
{
    public class AppContentRepository(DefaultDbContext context) : EfEntityRepositoryBase<AppContent, int>(context), IAppContentRepository
    {



    }
   
}
