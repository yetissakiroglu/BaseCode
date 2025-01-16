using Economy.Application.Repositories.AppSettingRepositories;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppSettingRepositories
{
    public class AppSettingRepository : EfEntityRepositoryBase<AppSetting, int>, IAppSettingRepository
    {
        public AppSettingRepository(AppDbContext context)
            : base(context)
        {



        }
    }
   
}
