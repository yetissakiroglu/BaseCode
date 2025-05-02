using Economy.Base.Persistence.BaseRepositories;
using Economy.Persistence.Contexts;

namespace Economy.Panel.Application.Repositories
{
    public class PanelAppUserTokenRepository : AppUserTokenBaseRepository
    {
        public PanelAppUserTokenRepository(DefaultDbContext _context) : base(_context)
        {
        }
    }

}
