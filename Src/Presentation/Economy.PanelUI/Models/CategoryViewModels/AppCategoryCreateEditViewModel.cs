using Economy.Domain.Enums;
using Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos;
using Economy.Panel.UI.Models.CategoryViewModels.AppCategoryTranslationViewModels;

namespace Economy.Panel.UI.Models.CategoryViewModels
{
    public class AppCategoryCreateEditViewModel
    {
        public int Id { get; set; }
        public ContentType ContentType { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<AppCategoryTranslationCreateEditViewModel> Translations { get; set; } = new();
    }
}
