using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Queries.AppPages;
using Economy.Persistence.Services;

namespace Economy.Application.Interfaces
{
    public interface IAppPageService
    {
        Task<ServiceResult<AppPageDto>> GetForReadAsync(GetAppPageQuery query);
        Task<ServiceResult<List<AppPageDto>>> WhereForReadAsync(GetAllAppPageQuery query);
    }
}
