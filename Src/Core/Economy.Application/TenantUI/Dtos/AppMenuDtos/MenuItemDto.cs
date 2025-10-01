using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos.AppMenuDtos
{
 
    public record MenuItemDto(
        int? Id,
        string Location,
        int? PageId,
        MenuOpenTarget OpenTarget,
        int SortOrder,
        bool IsActive,
        bool IsExternal,
        int? ParentId,
        List<MenuTranslationDto> Translations
    );


}
