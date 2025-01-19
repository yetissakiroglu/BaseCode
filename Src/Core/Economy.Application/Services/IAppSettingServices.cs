using Economy.Application.Dtos.WebSettingDtos;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Services;

namespace Economy.Application.Services
{
    public interface IAppSettingServices
    {
        Task<ServiceResult<AppSettingDto>> AppSettingDefaultAsync();


    }
}
