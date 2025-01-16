using Economy.Application.ApiDtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityCategories;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppContentServices
{
    public interface IAppCategoryService : IService<AppCategory, int>
    {

        //Task<ServiceResult<AppCategoryDto>> AppCategoryAsync(string languageCode, int CategoryId);
        //Task<ServiceResult<AppCategoryDto>> AppCategoryAsync(string languageCode, string url, int CategoryId);
        //Task<ServiceResult<List<AppCategoryDto>>> AppCategoriesAsync(string languageCode, List<int> CategoryIds);
        //Task<ServiceResult<List<AppCategoryDto>>> AppCategoriesAsync(string languageCode);
        //Task<ServiceResult<List<AppCategoryDto>>> AppCategoryContentsAsync(string languageCode, int CategoryId);

        Task<ServiceResult<ResponseCategoryDetailApiDto>> AppCategoryDetailAsync(string url);
        Task<ServiceResult<List<ResponseCategoryDetailApiDto>>> AppCategoriesAsync();

    }
}
