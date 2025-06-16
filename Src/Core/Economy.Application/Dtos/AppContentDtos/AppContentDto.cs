using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Enums;
using Economy.Panel.Application.Dtos.AppCategoryDtos;
using Economy.Panel.Application.Dtos.AppContentDtos.AppContentTranslationDtos;

namespace Economy.Panel.Application.Dtos.AppContentDtos
{
    public class AppContentDto
    {
        public int Id { get; set; }
        public string? WebThumbnailUrl { get; set; }
        public string? MobilThumbnailUrl { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
        // İlişkiler
        public int? AppCategoryId { get; set; } // Kategori ID'si     
        public virtual AppCategoryDto? AppCategory { get; set; }

        public List<AppContentTranslationDto> Translations { get; set; } = new();
    }
}
