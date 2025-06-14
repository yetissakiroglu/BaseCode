using Economy.Domain.Enums;
using Economy.Panel.Application.Dtos.AppCategoryDtos;
using Economy.Panel.Application.Dtos.AppContentDtos.AppContentTranslationDtos;
using Economy.Panel.UI.Models.CategoryViewModels;
using Economy.Panel.UI.Models.ContentViewModels.AppContentTranslationViewModels;

namespace Economy.Panel.UI.Models.ContentViewModels
{
    public class AppContentCreateEditViewModel
    {
        public int Id { get; set; }
        public string? WebThumbnailUrl { get; set; }
        public string? MobilThumbnailUrl { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
        // İlişkiler
        public int? AppCategoryId { get; set; } // Kategori ID'si     
        public virtual AppCategoryViewModel? AppCategory { get; set; }

        public List<AppContentTranslationCreateEditViewModel> Translations { get; set; } = new();
    }
}
