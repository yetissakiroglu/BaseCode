using Economy.Application.ApiDtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityAppPages;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppSectionServices
{
    public interface IAppSectionService:IService<AppSection,int>
    {
        Task<ServiceResult<ResponseSectionApiDto>> AppSectionAsync(int sectionId);



    }
}
