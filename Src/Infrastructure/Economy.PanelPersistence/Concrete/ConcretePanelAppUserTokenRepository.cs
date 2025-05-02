using Economy.Panel.Application.Repositories;
using Economy.Persistence.Contexts;

namespace Economy.Panel.Persistence.Repositories
{
    public class ConcretePanelAppUserTokenRepository : PanelAppUserTokenRepository
    {
        public ConcretePanelAppUserTokenRepository(DefaultDbContext _context) : base(_context)
        {
        }
    }
}
