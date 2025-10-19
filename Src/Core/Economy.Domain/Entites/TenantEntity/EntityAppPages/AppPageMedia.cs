using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class AppPageMedia : BaseEntity<int>
    {
        public int AppPageId { get; set; }
        public string MediaUrl { get; set; } = "";
        public bool IsCover { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<AppPageMediaTranslation> Translations { get; set; } = new List<AppPageMediaTranslation>();
    }
}
