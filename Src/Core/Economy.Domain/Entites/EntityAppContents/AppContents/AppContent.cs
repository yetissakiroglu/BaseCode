using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites.EntityAppContents.AppContents
{
    public class AppContent :BaseEntity<int>
    {

        public string? Thumbnail { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
        public PublicationStatus PublicationStatus { get; set; } // Yayın durumu


        // İlişkiler
        public int AppCategoryId { get; set; } // Kategori ID'si     
        public virtual AppCategory AppCategory { get; set; }
        public virtual ICollection<AppContentTranslation> Translations { get; set; } = new List<AppContentTranslation>();

    }
}
