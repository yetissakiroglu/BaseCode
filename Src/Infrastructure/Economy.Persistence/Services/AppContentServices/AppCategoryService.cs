using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppCategoryService(IAppCategoryRepository repository, IUnitOfWork unitOfWork): IAppCategoryService
    {
        private readonly IAppCategoryRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;



    }
}
