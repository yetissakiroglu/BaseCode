using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppLanguageDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppLanguageService
    {
        ResponseModel<IEnumerable<AppLanguageDto>> GetAllLanguage(bool isDeleted, bool isActive);
        ResponseModel<IEnumerable<AppLanguageDto>> GetAllLanguage(bool isDeleted);
        ResponseModel<AppLanguageDto> GetLanguage(int id,bool isDeleted);
        ResponseModel<AppLanguageDto> EditLanguage(AppLanguageEditDto model);
        ResponseModel<AppLanguageDto> CreateLanguage(AppLanguageCreateDto model);
        ResponseModel<AppLanguageDto> DeleteLanguage(int id);

    }
}
