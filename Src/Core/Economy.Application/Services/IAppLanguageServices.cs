using Economy.Application.Dtos.WebSettingDtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Persistence.Services;

namespace Economy.Application.Services
{
    public interface IAppLanguageServices : IService<AppLanguage,int>
    {
        Task<ServiceResult<AppLanguageDto>> AppLanguageAsync(string languageCode);
        Task<ServiceResult<AppLanguageDto>> AppLanguageDefaultAsync();
        Task<ServiceResult<List<AppLanguageDto>>> AppLanguageListAsync();

    }
}
