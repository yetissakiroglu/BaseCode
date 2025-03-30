using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Queries.AppPages;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppPageService
    {
        ResponseModel<AppPageDto> GetForReadDefaultPage(GetAppPageDefaultQuery query);
        ResponseModel<AppPageDto> GetForReadPageDefaultByLanguageCode(GetAppPageDefaultByLanguageCodeQuery query);
        ResponseModel<AppPageDto> GetForReadPageByLanguageCodeByUrl(GetAppPageByLanguageCodeByUrlQuery query);
    }
}
