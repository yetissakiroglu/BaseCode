using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppContentService(IAppContentRepository repository, IUnitOfWork unitOfWork)
        :  IAppContentService
    {
        private readonly IAppContentRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

    



    }
}
