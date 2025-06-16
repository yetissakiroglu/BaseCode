using Economy.Core.Tools.Result;
using Economy.Panel.Application.Dtos.AppCategoryDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppCategoryService
    {
        ServiceResult<List<AppCategoryDto>> GetAllCategories(bool isDeleted);
        ServiceResult<AppCategoryDto> GetCategory(int id, bool isDeleted);
        ServiceResult<AppCategoryDto> EditCategory(AppCategoryCreateEditDto model);
        ServiceResult<AppCategoryDto> CreateCategory(AppCategoryCreateEditDto model);
        ServiceResult<AppCategoryDto> DeleteCategory(int id);
    }
}
