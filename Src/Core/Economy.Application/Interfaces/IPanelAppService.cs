using Economy.Application.Dtos.AppDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Panel.Application.Dtos.AppDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppService 
    {
        ResponseModel<IEnumerable<AppDto>> Apps(bool isDeleted);
        Task<ServiceResult<AppDto>> GetAppById(int id);



        Task<ServiceResult<AppDto>> CreateApp(AppCreateEditDto model);
        Task<ResponseModel<AppDto>> EditApp(AppEditDto user);
        ResponseModel<AppDto> DeleteApp(int Id);
        ResponseModel<AppDto> GetApp(int Id, bool isDeleted);
    }
}
