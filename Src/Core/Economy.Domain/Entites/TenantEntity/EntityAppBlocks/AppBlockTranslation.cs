using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class AppBlockTranslation : BaseEntity<int>
    {
        public int AppBlockId { get; set; }
        public AppBlock AppBlock { get; set; } = null!;
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        // Dile bağlı metinler (hero başlık, açıklamalar, vb.)
        public string LocalizedJson { get; set; } = "{}";
    }
}
