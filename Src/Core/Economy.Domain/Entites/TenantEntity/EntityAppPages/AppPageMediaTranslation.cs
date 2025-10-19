using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class AppPageMediaTranslation : BaseEntity<int>
    {
        public int AppPageMediaId { get; set; }
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;
        public string? Alt { get; set; }
        public string? Caption { get; set; }
    }
}
