using Economy.Core.Extensions;
using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppContents;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Enums;
using Economy.Domain.Models;

namespace Economy.Domain.Entites.EntityPages
{
    public class AppContent :BaseEntity<int>
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }                                 
        public bool IsExternal { get; set; }
        public string Url { get; set; }

        // SEO alanları
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
        public PublicationStatus PublicationStatus { get; set; } // Yayın durumu


        // İlişkiler
        public int AppCategoryId { get; set; } // Kategori ID'si     
        public virtual AppCategory AppCategory { get; set; }
    
    }
}
