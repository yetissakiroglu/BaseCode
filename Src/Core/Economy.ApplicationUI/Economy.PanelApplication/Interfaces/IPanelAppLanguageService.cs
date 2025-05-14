using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppLanguageDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppLanguageService
    {
        ResponseModel<IEnumerable<AppLanguageDto>> GetAllLanguage(bool isDeleted, bool isActive);
        ResponseModel<IEnumerable<AppLanguageDto>> GetAllLanguage(bool isDeleted);
        ResponseModel<AppLanguageDto> GetLanguage(int id,bool isDeleted);

    }
}
