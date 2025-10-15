using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class BlockGroupTranslation : BaseEntity<int>
    {
        public int BlockGroupId { get; set; }
        public BlockGroup BlockGroup { get; set; }
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
