using Economy.Domain.Enums;

namespace Economy.Application.Dtos.AppContentDtos
{
    public class AppContentDto
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
  
        public virtual AppCategoryDto AppCategory { get; set; }
    }
}
