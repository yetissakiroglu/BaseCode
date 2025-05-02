using Economy.Application.Repositories.AppSlideRepositories;
using Economy.Domain.Entites.EntitySlides;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.AppSlideRepositories
{
    public class AppSlideRepository(DefaultDbContext context) : EfEntityRepositoryBase<AppSlide>(context), IAppSlideRepository
    {



    }

}
