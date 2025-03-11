using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Queries.AppPages;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppPageService
    {
        Task<ResponseModel<AppPageDto>> GetForReadDefaultPageAsync(GetAppPageQuery query);
        Task<ResponseModel<AppPageDto>> GetForReadDefaultPageByLanguageCodeAsync(GetAppPageByLanguageCodeQuery query);

    }
}
