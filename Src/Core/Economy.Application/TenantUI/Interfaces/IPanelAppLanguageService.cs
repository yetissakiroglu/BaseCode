using Economy.Application.TenantUI.Dtos.AppLanguageDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppLanguageService
    {
        ServiceResult<List<AppLanguageDto>> GetAllLanguage(bool isDeleted, bool isActive);
        ServiceResult<List<AppLanguageDto>> GetAllLanguage(bool isDeleted);
        ServiceResult<AppLanguageDto> GetLanguage(int id,bool isDeleted);
        ServiceResult<AppLanguageDto> EditLanguage(AppLanguageCreateEditDto model);
        ServiceResult<AppLanguageDto> CreateLanguage(AppLanguageCreateEditDto model);
        ServiceResult<AppLanguageDto> DeleteLanguage(int id);
    }
}
