using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class BlockItem : BaseEntity<int>
    {
        public int BlockGroupId { get; set; }
        public BlockGroup BlockGroup { get; set; }


        public string Title { get; set; }
        public string? Summary { get; set; }
        public string CoverImage { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; }

        
        public ImageMode? ImageModeOverride { get; set; }
        public ICollection<BlockItemImage> Gallery { get; set; } = new List<BlockItemImage>();


        public LinkType LinkType { get; set; } = LinkType.None;
        public int? LinkedPageId { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Target { get; set; } // _self, _blank


        public BlockColumns? ColumnsOverride { get; set; }
    }
}
