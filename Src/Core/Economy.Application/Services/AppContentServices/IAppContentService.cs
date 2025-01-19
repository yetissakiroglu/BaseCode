using Economy.Application.ApiDtos;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppContentServices
{
    public interface IAppContentService 
    {
         //Api              
        Task<ServiceResult<List<ResponseHeadlineApiDto>>> AppContentHeadlineListAsync(int countRow);
        Task<ServiceResult<List<ResponseFeaturedApiDto>>> AppContentFeaturedListAsync(int countRow);
        Task<ServiceResult<List<ResponseBreakingNewsApiDto>>> AppContentBreakingNewsListAsync(int countRow);
        Task<ServiceResult<ResponseContentDetailApiDto>> AppContentDetailAsync(int appContentId);
    }
}
