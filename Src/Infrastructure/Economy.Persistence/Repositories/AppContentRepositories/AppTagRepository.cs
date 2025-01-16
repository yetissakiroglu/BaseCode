using Economy.Application.Repositories.AppContentRepositories;
using Economy.Domain.Entites.EntityAppContents;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppContentRepositories
{
    public class AppTagRepository(AppDbContext context) : EfEntityRepositoryBase<AppTag, int>(context), IAppTagRepository
    {







    }

}