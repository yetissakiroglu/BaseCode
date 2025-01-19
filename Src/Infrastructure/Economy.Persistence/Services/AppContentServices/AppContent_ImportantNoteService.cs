using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppContent_ImportantNoteService(IAppContent_ImportantNoteRepository repository, IUnitOfWork unitOfWork)
        :  IAppContent_ImportantNoteService
    {


    }
}
