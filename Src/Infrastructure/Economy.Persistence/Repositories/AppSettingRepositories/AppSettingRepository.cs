using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppMenuRepositories
{
    public class AppSettingRepository(DefaultDbContext context) : EfEntityRepositoryBase<AppSetting ,int>(context), IAppSettingRepository
    {



    }

}