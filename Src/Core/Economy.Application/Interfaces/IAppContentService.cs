using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Queries.AppContents;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppContentService
    {
        ResponseModel<AppContentDto> GetForReadByLanguageCodeByAppContentId(GetAppContentByLanguageCodeByAppContentIdQuery query);
        ResponseModel<AppContentDto> GetForReadByLanguageCodeByUrl(GetAppContentByLanguageCodeByUrlQuery query);
        ResponseModel<List<AppContentDto>> WhereForReadByLanguageCodeByAppContentIds(GetAllAppContentByLanguageCodeByAppContentIdsQuery query);

    }
}
