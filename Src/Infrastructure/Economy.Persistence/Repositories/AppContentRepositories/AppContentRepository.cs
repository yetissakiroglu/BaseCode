using Economy.Application.Repositories.AppContentRepositories;
using Economy.Domain.Entites.EntityPages;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppContentRepositories
{
    public class AppContentRepository : EfEntityRepositoryBase<AppContent, int>, IAppContentRepository
    {
        public AppContentRepository(AppDbContext context)
            : base(context)
        {



        }
    }

}