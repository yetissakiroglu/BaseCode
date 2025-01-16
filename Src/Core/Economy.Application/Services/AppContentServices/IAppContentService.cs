using Economy.Application.ApiDtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityPages;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppContentServices
{
    public interface IAppContentService : IService<AppContent, int>
    {
         //Api              
        Task<ServiceResult<List<ResponseHeadlineApiDto>>> AppContentHeadlineListAsync(int countRow);
        Task<ServiceResult<List<ResponseFeaturedApiDto>>> AppContentFeaturedListAsync(int countRow);
        Task<ServiceResult<List<ResponseBreakingNewsApiDto>>> AppContentBreakingNewsListAsync(int countRow);
        Task<ServiceResult<ResponseContentDetailApiDto>> AppContentDetailAsync(int appContentId);
    }
}
