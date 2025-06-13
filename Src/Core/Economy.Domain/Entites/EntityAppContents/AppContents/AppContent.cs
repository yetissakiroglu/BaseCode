using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites.EntityAppContents.AppContents
{
    public class AppContent :BaseEntity<int>
    {
        public string? WebThumbnailUrl { get; set; }
        public string? MobilThumbnailUrl { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
                                                                           // İlişkiler
        public int? AppCategoryId { get; set; } // Nullable hale getirildi
        public virtual AppCategory? AppCategory { get; set; } // Optional navigation property
        public virtual ICollection<AppContentTranslation> Translations { get; set; } = new List<AppContentTranslation>();

    }
}
