using Economy.Domain.Enums;

namespace Economy.Application.Dtos.AppContentDtos
{
    public class AppContentDto
    {
        public string? Thumbnail { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
        public PublicationStatus PublicationStatus { get; set; } // Yayın durumu
        public int AppCategoryId { get; set; }
        public AppCategoryDto AppCategory { get; set; }
        public List<AppContentTranslationDto> Translations { get; set; } = new();
        
    }
}
