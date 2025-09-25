using Economy.Core.Enums;

namespace Economy.Application.Dtos.AppMenuDtos
{
    public record MenuTranslationDto(
     int AppLanguageId,
     string Title,
     string? Url // IsExternal=true iken gerekli
 );

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

    public record MenuNodeTranslationDto(int AppLanguageId, string Title, string? Url);


}
