using Economy.Application.ApiDtos;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppSectionServices
{
    public interface IAppPageService 
    {
        Task<ServiceResult<ResponsePageDetailApiDto>> AppPageDetailAsync(string url);
        Task<ServiceResult<ResponsePageDetailApiDto>> AppPageDetailDefaultAsync();


    }
}
