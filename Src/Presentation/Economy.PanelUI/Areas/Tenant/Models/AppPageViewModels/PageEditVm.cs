using Economy.Domain.Entites.TenantEntity.EntityAppContents;

namespace Economy.Panel.UI.Areas.Tenant.Models.AppPageViewModels
{
    public class PageEditVm
    {
        public int? Id { get; set; }
        public string Slug { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Draft;
        public List<PageTranslationVm> Translations { get; set; } = new();
        public List<PageBlockVm> Blocks { get; set; } = new();
    }
    public class PageTranslationVm
    {
        public int? Id { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; } = "";
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
    }
    public class PageBlockVm
    {
        public int? Id { get; set; }
        public BlockType Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Draft;

        public string SharedJson { get; set; } = "{}";
        public int? LibraryBlockId { get; set; }
        public bool IsLinkedToLibrary { get; set; }

        public List<PageBlockTranslationVm> Translations { get; set; } = new();
    }
    public class PageBlockTranslationVm
    {
        public int? Id { get; set; }
        public int LanguageId { get; set; }
        public string LocalizedJson { get; set; } = "{}";
    }

    public class PageBlockIndexVm
    {
        public List<PageBlockListItemVm> Items { get; set; } = new();
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public int? FilterPageId { get; set; }
        public BlockType? FilterType { get; set; }
        public string? Q { get; set; }
    }
    public class PageBlockListItemVm
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string PageSlug { get; set; } = "";
        public BlockType Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public int? LibraryBlockId { get; set; }
        public bool IsLinkedToLibrary { get; set; }
        public ContentStage Stage { get; set; }
    }


}
