using Economy.Application.Dtos.WebSettingDtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Services;

namespace Economy.Application.Services
{
	public interface IAppSettingServices : IService<AppSetting,int>
    {
        Task<ServiceResult<AppSettingDto>> AppSettingDefaultAsync();


    }
}
