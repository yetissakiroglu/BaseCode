using Economy.Base.Application.BaseRepositories;
using Economy.Domain.Entites.AppEntities;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Base.Persistence.BaseRepositories
{
    public class AppBaseRepository : EfEntityRepositoryBase<App>, IAppBaseRepository
    {
        public AppBaseRepository(DefaultDbContext _context) : base(_context)
        {

        }
    }

}
