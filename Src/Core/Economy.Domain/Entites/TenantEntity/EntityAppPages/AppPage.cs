using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class AppPage : BaseEntity<int>
    {
        public ContentItemType Type { get; set; } = ContentItemType.Page;
        public int? AppPageId { get; set; }
        public bool IsHomepage { get; set; } = false;
        public string? CoverImageUrl { get; set; }
        public string? OgImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public PageStage Stage { get; set; } = PageStage.Draft;
        public int SortOrder { get; set; } = 0;
        public DateTime? PublishAtUtc { get; set; }
        public ICollection<AppPageTranslation> Translations { get; set; } = new List<AppPageTranslation>();
        public ICollection<AppPageMedia> Medias { get; set; } = new List<AppPageMedia>();
    }
}
