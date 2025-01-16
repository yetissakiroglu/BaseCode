using Economy.Application.Repositories.AppContentRepositories;
using Economy.Domain.Entites.EntityCategories;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppContentRepositories
{
    public class AppCategoryRepository(AppDbContext context) : EfEntityRepositoryBase<AppCategory, int>(context), IAppCategoryRepository
    {
    }

}