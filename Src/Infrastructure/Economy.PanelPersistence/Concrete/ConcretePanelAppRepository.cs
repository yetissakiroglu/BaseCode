using Economy.Panel.Application.Repositories;
using Economy.Persistence.Contexts;

namespace Economy.Panel.Persistence.Repositories
{
    public class ConcretePanelAppRepository : PanelAppRepository
    {
        public ConcretePanelAppRepository(DefaultDbContext _context) : base(_context)
        {
        }
    }
}
