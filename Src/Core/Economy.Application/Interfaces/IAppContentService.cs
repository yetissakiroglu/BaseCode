using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Queries.AppContents;
using Economy.Application.Queries.AppPages;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppContentService
    {
        Task<ResponseModel<AppContentDto>> GetForReadByLanguageCodeByAppContentIdAsync(GetAppContentByLanguageCodeByAppContentIdQuery query);
        Task<ResponseModel<AppContentDto>> GetForReadByLanguageCodeByUrlAsync(GetAppContentByLanguageCodeByUrlQuery query);

        Task<ResponseModel<List<AppContentDto>>> WhereForReadByLanguageCodeByAppContentIdsAsync(GetAllAppContentByLanguageCodeByAppContentIdsQuery query);

    }
}
