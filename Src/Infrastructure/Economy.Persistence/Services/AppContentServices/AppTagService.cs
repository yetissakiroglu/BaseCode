using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppTagService(IAppTagRepository repository, IUnitOfWork unitOfWork)
        :  IAppTagService
    {


    }
}
