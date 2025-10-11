using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class BlockItemImage : BaseEntity<int>
    {
        public int BlockItemId { get; set; }
        public BlockItem BlockItem { get; set; }
        public int SortOrder { get; set; } = 0;

        public string ImageUrl { get; set; }
        public string? AltText { get; set; }
    }
}
