using Economy.Application.Repositories.AppContentRepositories;
using Economy.Domain.Entites.EntityAppContents;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppContentRepositories
{
    public class AppContent_ImportantNoteRepository(AppDbContext context) : EfEntityRepositoryBase<AppContent_ImportantNote, int>(context), IAppContent_ImportantNoteRepository
    {


    }

}