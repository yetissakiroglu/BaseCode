using Economy.Panel.Application.Repositories;
using Economy.Persistence.Contexts;

namespace Economy.Panel.Persistence.Repositories
{
    public class ConcretePanelAppSettingRepository : PanelAppSettingRepository
    {
        public ConcretePanelAppSettingRepository(HotelDbContext _context) : base(_context)
        {
        }
    }
}
