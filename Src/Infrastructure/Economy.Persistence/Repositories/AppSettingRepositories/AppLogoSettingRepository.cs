using Economy.Application.Repositories.AppSettingRepositories;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppMenuRepositories
{
    public class AppLogoSettingRepository(AppDbContext context) : EfEntityRepositoryBase<AppLogoSetting ,int>(context), IAppLogoSettingRepository
    {



    }

}