using Economy.Base.Persistence.BaseRepositories;
using Economy.Persistence.Contexts;

namespace Economy.Panel.Application.Repositories
{
    public abstract class PanelAppSettingRepository : AppSettingBaseRepository
    {
        protected PanelAppSettingRepository(HotelDbContext _context) : base(_context)
        {
        }
    }
  
}
