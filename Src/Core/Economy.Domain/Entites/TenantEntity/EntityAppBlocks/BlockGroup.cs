using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class BlockGroup : BaseEntity<int>
    {
        public int? PageId { get; set; } // doğrudan sayfaya bağlamak istersen


        public string Code { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; }

        
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public ImageMode DefaultImageMode { get; set; } = ImageMode.CoverOnly;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;


        public ICollection<BlockItem> Items { get; set; } = new List<BlockItem>();
    }
}
