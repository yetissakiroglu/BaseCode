using Economy.Base.Persistence.BaseRepositories;
using Economy.Persistence.Contexts;

namespace Economy.Panel.Application.Repositories
{
    public abstract class PanelAppRepository : AppBaseRepository
    {
        protected PanelAppRepository(DefaultDbContext _context) : base(_context)
        {
        }
    }
}
