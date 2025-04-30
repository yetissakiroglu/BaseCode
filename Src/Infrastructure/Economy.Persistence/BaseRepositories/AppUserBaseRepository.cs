using Economy.Domain.Entites.Identities;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.BaseRepositories
{
    public abstract class AppUserBaseRepository : EfEntityRepositoryBase<AppUser, int>
    {
        protected AppUserBaseRepository(DefaultDbContext _context) : base(_context)
        {
        }
    }
}
