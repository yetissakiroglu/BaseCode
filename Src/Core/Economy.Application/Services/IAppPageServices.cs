using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityAppPages;

namespace Economy.Application.Services
{
    public interface IAppPageServices : IService<AppPage,int>
    {

        //Task<ServiceResult<AppPageDto>> AppPageIsHomePageAsync();
        //Task<ServiceResult<AppPageDto>> AppPageAsync(string url);

    }
}
