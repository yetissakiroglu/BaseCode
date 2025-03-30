using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Application.Queries.AppLanguages;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppLanguageService
    {
        ResponseModel<AppLanguageDto> GetDefaultForRead(GetAppLanguageByDefaultQuery query);
        ResponseModel<List<AppLanguageDto>> GetAllForRead(GetAllAppLanguageQuery query);
    }
}
