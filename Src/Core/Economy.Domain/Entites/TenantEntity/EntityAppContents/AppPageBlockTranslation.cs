using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppContents
{
    public class AppPageBlockTranslation: BaseEntity<int>
    {
        public int AppPageBlockId { get; set; }
        public AppPageBlock AppPageBlock { get; set; } = null!;

        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        // Dile bağlı metinler (hero başlık, açıklamalar, vb.)
        public string LocalizedJson { get; set; } = "{}";
    }
}
