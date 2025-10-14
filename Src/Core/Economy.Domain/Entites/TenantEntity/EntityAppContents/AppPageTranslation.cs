using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppContents
{
    public class AppPageTranslation:BaseEntity<int>
    {
        public int Id { get; set; }
        public int AppPageId { get; set; }
        public AppPage AppPage { get; set; } = null!;

        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        [MaxLength(160)] public string Title { get; set; } = null!;
        [MaxLength(160)] public string? SeoTitle { get; set; }
        [MaxLength(300)] public string? SeoDescription { get; set; }
    }
}
