using Economy.Application.Repositories.AppSectionRepositories;
using Economy.Application.Services.AppSectionServices;
using Economy.Application.UnitOfWorks;

namespace Economy.Persistence.Services.AppSectionServices
{
    public class AppSectionService(IAppSectionRepository repository, IUnitOfWork unitOfWork)
        : IAppSectionService
    {



       
    }
}
