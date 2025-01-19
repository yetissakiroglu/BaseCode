using Economy.Application.ApiDtos;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppSectionServices
{
    public interface IAppSectionService
    {
        Task<ServiceResult<ResponseSectionApiDto>> AppSectionAsync(int sectionId);



    }
}
