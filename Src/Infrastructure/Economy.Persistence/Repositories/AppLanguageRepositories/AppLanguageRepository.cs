using Economy.Application.Repositories.AppLanguageRepositories;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppLanguageRepositories
{
    public class AppLanguageRepository(AppDbContext context) : EfEntityRepositoryBase<AppLanguage, int>(context), IAppLanguageRepository
    {



    }
 
}
