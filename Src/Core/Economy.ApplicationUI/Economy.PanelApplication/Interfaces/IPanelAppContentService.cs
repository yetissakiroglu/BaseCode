using Economy.Core.Tools.Result;
using Economy.Panel.Application.Dtos.AppContentDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppContentService
    {
        ServiceResult<List<AppContentDto>> GetAllContent(bool isDeleted);
        ServiceResult<AppContentDto> GetContent(int id, bool isDeleted);
        ServiceResult<AppContentDto> EditContent(AppContentCreateEditDto model);
        ServiceResult<AppContentDto> CreateContent(AppContentCreateEditDto model);
        ServiceResult<AppContentDto> DeleteContent(int id);


        //ServiceResult<List<AppContentTranslationDto>> GetAllTranslations(int contentId, bool isDeleted);
        //ServiceResult<AppContentTranslationDto> GetTranslation(int id, bool isDeleted);
        //ServiceResult<AppContentTranslationDto> EditTranslation(AppContentTranslationCreateEditDto model);
        //ServiceResult<AppContentTranslationDto> CreateTranslation(AppContentTranslationCreateEditDto model);
        //ServiceResult<AppContentTranslationDto> DeleteTranslation(int id);
    }
}
