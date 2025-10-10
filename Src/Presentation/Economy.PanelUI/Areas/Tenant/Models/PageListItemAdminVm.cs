using Economy.Core.Enums;

namespace Economy.Panel.UI.Areas.Tenant.Models
{

    public sealed class PageListItemAdminVm
    {
        public int Id { get; set; }
        public string? ParentTitle { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public DateTime? PublishAtUtc { get; set; }
        public int SortOrder { get; set; }
    }

    public sealed class PageParentOptionVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
    }

    public sealed class PageTranslationVm
    {
        public int? Id { get; set; }
        public int LanguageId { get; set; }
        public string LanguageCode { get; set; } = default!;
        public string? Slug { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    public sealed class PageEditVm
    {
        public int? Id { get; set; }                // ContentItem.Id
        public int? OwnerId { get; set; }           // Parent
        public bool IsActive { get; set; } = true;
        public DateTime? PublishAtUtc { get; set; }
        public int SortOrder { get; set; }
        public short Type { get; set; } = 1;        // Page
        public string? Image { get; set; }
        public string? OgImage { get; set; }
        public List<PageTranslationVm> Translations { get; set; } = new();
    }

    // --- Blocks ---
    public sealed class BlockListItemVm
    {
        public int Id { get; set; }
        public string Template { get; set; } = default!;
        public int SortOrder { get; set; }
        public string? Title { get; set; }
    }

    public sealed class BlockEditVm
    {
        public int? Id { get; set; }                // ContentItem.Id (Type=Block)
        public int PageId { get; set; }             // OwnerId
        public BlockTemplate? BlockTemplate { get; set; }    // enum value
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // tek-dil giriş (tl;dr: bloklar da translate’li ise aşağıdaki alanları dil sekmeli yapabilirsiniz)
        public int LanguageId { get; set; }
        public string LanguageCode { get; set; } = default!;
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public string? Image { get; set; }
        public string? OgImage { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? JsonData { get; set; }
    }

    // --- Gallery ---
    public sealed class MediaListItemVm
    {
        public int Id { get; set; }
        public string Url { get; set; } = default!;
        public int SortOrder { get; set; }
        public string? Alt { get; set; }
        public string? Caption { get; set; }
    }

    public sealed class MediaEditVm
    {
        public int? Id { get; set; }
        public int PageId { get; set; }
        public string Url { get; set; } = default!;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public int LanguageId { get; set; }
        public string LanguageCode { get; set; } = default!;
        public string? Alt { get; set; }
        public string? Caption { get; set; }
    }

    // --- Snippets ---
    public sealed class SnippetListItemVm
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public bool IsActive { get; set; }
    }

    public sealed class SnippetEditVm
    {
        public int? Id { get; set; }                // ContentItem.Id (Type=Snippet)
        public string Code { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public List<PageTranslationVm> Translations { get; set; } = new();
    }

}
