using Economy.Base.Application.BaseRepositories;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Base.Persistence.BaseRepositories
{
    public class AppSettingBaseRepository : EfEntityRepositoryBase<AppSetting>, IAppSettingBaseRepository
    {
        public AppSettingBaseRepository(HotelDbContext _context) : base(_context)
        {

        }
    }

}
