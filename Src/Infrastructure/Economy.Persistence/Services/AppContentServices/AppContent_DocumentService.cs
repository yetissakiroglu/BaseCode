using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppContent_DocumentService(IAppContent_DocumentRepository repository, IUnitOfWork unitOfWork)
        :  IAppContent_DocumentService
    {
        private readonly IAppContent_DocumentRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


    }
}
