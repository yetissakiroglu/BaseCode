using Economy.Application.Repositories.AppContentRepositories;
using Economy.Domain.Entites.EntityPages;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppContentRepositories
{
    public class AppContentRepository(AppDbContext context) : EfEntityRepositoryBase<AppContent, int>(context), IAppContentRepository
    {



    }
   
}
