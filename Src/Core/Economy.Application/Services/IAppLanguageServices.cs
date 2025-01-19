using Economy.Application.Dtos.WebSettingDtos;
using Economy.Persistence.Services;

namespace Economy.Application.Services
{
    public interface IAppLanguageServices 
    {
        Task<ServiceResult<AppLanguageDto>> AppLanguageAsync(string languageCode);
        Task<ServiceResult<AppLanguageDto>> AppLanguageDefaultAsync();
        Task<ServiceResult<List<AppLanguageDto>>> AppLanguageListAsync();

    }
}
