using Economy.Application.ApiDtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityAppPages;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppSectionServices
{
    public interface IAppPageService : IService<AppPage,int>
    {
        Task<ServiceResult<ResponsePageDetailApiDto>> AppPageDetailAsync(string url);
        Task<ServiceResult<ResponsePageDetailApiDto>> AppPageDetailDefaultAsync();


    }
}
