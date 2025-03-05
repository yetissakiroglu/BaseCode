using Economy.Application.Dtos.AppSettingDtos;
using Economy.Application.Queries.AppSettings;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppSettingService
    {
        Task<ResponseModel<AppSettingDto>> GetForReadAsync(GetAppSettingQuery query);

    }
}
