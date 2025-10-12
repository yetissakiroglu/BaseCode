using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    // İçerik ana kaydı: Page / Snippet / Block
    [Index(nameof(Type))]
    public class ContentItem : BaseEntity<int>
    {
        public ContentItemType Type { get; set; } = ContentItemType.Page;

        public int? OwnerId { get; set; }

        public bool IsHomepage { get; set; } = false;

        // Görsel/Buton/SEO
        [MaxLength(500)]
        public string? Image { get; set; }

        [MaxLength(500)]
        public string? OgImage { get; set; }

        // Listeleme/SEO
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        public DateTime? PublishAtUtc { get; set; }
        

        // Navigations
        public ICollection<ContentItemTranslation> Translations { get; set; } = new List<ContentItemTranslation>();
        public ICollection<ContentMedia> Media { get; set; } = new List<ContentMedia>();
    }

}
