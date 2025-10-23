using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class AppBlockGroupTranslation : BaseEntity<int>
    {
        public int AppBlockGroupId { get; set; }
        public AppBlockGroup AppBlockGroup { get; set; }
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
