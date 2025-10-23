using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class AppBlockGroupBlockTranslation : BaseEntity<int>
    {
        public int AppBlockGroupBlockId { get; set; }
        public AppBlockGroupBlock AppBlockGroupBlock { get; set; } = null!;
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        // Dile bağlı metinler (hero başlık, açıklamalar, vb.)
        public string LocalizedJson { get; set; } = "{}";
    }
}
