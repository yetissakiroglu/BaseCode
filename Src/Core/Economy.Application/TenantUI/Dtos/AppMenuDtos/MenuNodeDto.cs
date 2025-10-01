using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos.AppMenuDtos
{
    public record MenuNodeDto(
 int Id,
 string Location,
 int? PageId,
 MenuOpenTarget OpenTarget,
 int SortOrder,
 bool IsActive,
 bool IsExternal,
 int? ParentId,
 List<MenuNodeTranslationDto> Translations,
 List<MenuNodeDto> Children
);

}
