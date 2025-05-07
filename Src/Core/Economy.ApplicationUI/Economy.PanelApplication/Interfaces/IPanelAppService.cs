using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppService
    {
        ResponseModel<IEnumerable<AppDto>> Apps(bool isDeleted);
        Task<ResponseModel<AppDto>> CreateApp(AppCreateDto user);
    }
}
