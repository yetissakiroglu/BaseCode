using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Queries.AppPages;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppPageService
    {
        Task<ResponseModel<AppPageDto>> GetForReadDefaultPageAsync(GetAppPageDefaultQuery query);
        Task<ResponseModel<AppPageDto>> GetForReadPageDefaultByLanguageCodeAsync(GetAppPageDefaultByLanguageCodeQuery query);
        Task<ResponseModel<AppPageDto>> GetForReadPageByLanguageCodeByUrlAsync(GetAppPageByLanguageCodeByUrlQuery query);

    }
}
