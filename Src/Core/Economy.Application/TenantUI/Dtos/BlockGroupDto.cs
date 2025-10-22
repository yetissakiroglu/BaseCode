using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos
{
    public class BlockGroupDto
    {
        public int? Id { get; set; }
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public List<BlockGroupTranslationDto> Translations { get; set; } = new();
        public List<BlockGroupBlockVm> Blocks { get; set; } = new();
    }

    public class BlockGroupBlockVm
    {
        public int? Id { get; set; }
        public BlockType Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Draft;
        public string SharedJson { get; set; } = "{}";
        public List<BlockGroupBlockTranslationVm> Translations { get; set; } = new();
    }

    public class BlockGroupBlockTranslationVm
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
        public int BlockGroupId { get; set; }
        public BlockType Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public ContentStage Stage { get; set; }
    }

}
