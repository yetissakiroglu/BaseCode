using Economy.Domain.Enums;
using Economy.Panel.Application.Dtos.AppCategoryDtos;
using Economy.Panel.Application.Dtos.AppContentDtos.AppContentTranslationDtos;

namespace Economy.Panel.Application.Dtos.AppContentDtos
{
    public class AppContentCreateEditDto
    {
        public int Id { get; set; }
        public string? WebThumbnailUrl { get; set; }
        public string? MobilThumbnailUrl { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
        // İlişkiler
        public int? AppCategoryId { get; set; } // Kategori ID'si     
        public virtual AppCategoryDto? AppCategory { get; set; }

        public List<AppContentTranslationCreateEditDto> Translations { get; set; } = new();
    }
}
