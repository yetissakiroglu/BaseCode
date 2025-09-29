using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.EntityAppNewPages
{
    // İçerik ana kaydı: Page / Snippet / Block
    [Index(nameof(Type))]
    [Index(nameof(OwnerType), nameof(OwnerId))]
    [Index(nameof(Code))] // Snippet çağırma kodu için
    public class ContentItem : BaseEntity<int>
    {
        public ContentItemType Type { get; set; } = ContentItemType.Page;

        // Sahiplik (Block için Content’e bağlanır; opsiyonel Room/Campaign)
        public ContentOwnerType OwnerType { get; set; } = ContentOwnerType.None;
        public int? OwnerId { get; set; }

        // Sadece Block’larda anlamlı
        public BlockTemplate? BlockTemplate { get; set; }

        public bool IsHomepage { get; set; } = true;

        
        // Listeleme/SEO
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        public DateTime? PublishAtUtc { get; set; }

        // Snippet çağırma kodu (örn: "PET_POLICY_NOTE")
        [MaxLength(100)]
        public string? Code { get; set; }

        // Navigations
        public ICollection<ContentItemTranslation> Translations { get; set; } = new List<ContentItemTranslation>();
        public ICollection<ContentMedia> Media { get; set; } = new List<ContentMedia>();
    }

    // İçerik çevirileri (dile bağlı alanlar)
    [Index(nameof(ContentItemId), nameof(LanguageId), IsUnique = true)] // aynı içerik + aynı dil = tek kayıt
    [Index(nameof(LanguageId), nameof(Slug))]                           // slug aramaları
    public class ContentItemTranslation : BaseEntity<int>
    {
        // FK’ler
        public int ContentItemId { get; set; }
        public int LanguageId { get; set; }

        // URL için dil bazlı slug (Page/Block)
        [MaxLength(200)]
        public string? Slug { get; set; }

        // Metin alanları
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Summary { get; set; }

        public string? Body { get; set; }

        // Görsel/Buton/SEO
        [MaxLength(500)]
        public string? Image { get; set; }

        [MaxLength(100)]
        public string? ButtonText { get; set; }

        [MaxLength(500)]
        public string? ButtonUrl { get; set; }

        [MaxLength(200)]
        public string? MetaTitle { get; set; }

        [MaxLength(300)]
        public string? MetaDescription { get; set; }

        [MaxLength(500)]
        public string? OgImage { get; set; }

        // Esnek alan: badge, priceFrom, validFrom, category, custom ayarlar...
        public string? JsonData { get; set; }

        // Durum
        public bool IsActive { get; set; } = true;

        // Navigations (opsiyonel, Fluent'te bağlanır)
        // public ContentItem? ContentItem { get; set; }
        // public AppLanguage? Language { get; set; }
    }

    // İçeriğe bağlı medya (galeri/kapak)
    [Index(nameof(OwnerType), nameof(OwnerId))]
    public class ContentMedia : BaseEntity<int>
    {
        public MediaOwnerType OwnerType { get; set; } = MediaOwnerType.Content;
        public int OwnerId { get; set; }

        [Required, MaxLength(500)]
        public string Url { get; set; } = "";

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Navigations
        public ICollection<ContentMediaTranslation> Translations { get; set; } = new List<ContentMediaTranslation>();
    }

    // Medya çevirileri (alt/caption gibi dil bazlı)
    [Index(nameof(ContentMediaId), nameof(LanguageId), IsUnique = true)]
    public class ContentMediaTranslation : BaseEntity<int>
    {
        public int ContentMediaId { get; set; }
        public int LanguageId { get; set; }

        [MaxLength(200)]
        public string? Alt { get; set; }

        [MaxLength(300)]
        public string? Caption { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigations (opsiyonel)
        // public ContentMedia? ContentMedia { get; set; }
        // public AppLanguage? Language { get; set; }
    }
}
