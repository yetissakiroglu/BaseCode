using Economy.Base.Application.BaseRepositories;
using Economy.Domain.Entities.Identity;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Base.Persistence.BaseRepositories
{
    public class AppUserTokenBaseRepository : EfEntityRepositoryBase<AppUserToken>, IAppUserTokenBaseRepository
    {
        public AppUserTokenBaseRepository(DefaultDbContext _context) : base(_context)
        {
        }
    }
}
