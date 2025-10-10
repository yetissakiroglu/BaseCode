using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    // İçerik ana kaydı: Page / Snippet / Block
    [Index(nameof(Type))]
    [Index(nameof(Code))] // Snippet çağırma kodu için
    public class ContentItem : BaseEntity<int>
    {
        public ContentItemType Type { get; set; } = ContentItemType.Page;

        public int? OwnerId { get; set; }

        // Sadece Block’larda anlamlı
        public BlockTemplate? BlockTemplate { get; set; }

        public bool IsHomepage { get; set; } = true;

        // Görsel/Buton/SEO
        [MaxLength(500)]
        public string? Image { get; set; }

        [MaxLength(500)]
        public string? OgImage { get; set; }

        // Listeleme/SEO
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        public DateTime? PublishAtUtc { get; set; }

        // Snippet çağırma kodu (örn: "PET_POLICY_NOTE")
        [MaxLength(100)]
        public string? Code { get; set; }
        public string? JsonData { get; set; }

        // Navigations
        public ICollection<ContentItemTranslation> Translations { get; set; } = new List<ContentItemTranslation>();
        public ICollection<ContentMedia> Media { get; set; } = new List<ContentMedia>();
    }

}
