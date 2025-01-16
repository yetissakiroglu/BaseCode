using Economy.Application.Repositories.AppContentRepositories;
using Economy.Domain.Entites.EntityAppContents;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppContentRepositories
{
    public class AppContent_DocumentRepository(AppDbContext context) : EfEntityRepositoryBase<AppContent_Document, int>(context), IAppContent_DocumentRepository
    {


    }

}