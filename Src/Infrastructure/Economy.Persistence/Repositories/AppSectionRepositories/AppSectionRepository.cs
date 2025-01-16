using Economy.Application.Repositories.AppSectionRepositories;
using Economy.Domain.Entites.EntityAppPages;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppSectionRepositories
{
    public class AppSectionRepository(AppDbContext context) : EfEntityRepositoryBase<AppSection, int>(context), IAppSectionRepository
    {
    }

}