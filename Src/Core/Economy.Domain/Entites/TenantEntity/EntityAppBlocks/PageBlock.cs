using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class PageBlock : BaseEntity<int>
    {
        public int PageId { get; set; }
        public int BlockGroupId { get; set; }
        public int SortOrder { get; set; }
    }
}
