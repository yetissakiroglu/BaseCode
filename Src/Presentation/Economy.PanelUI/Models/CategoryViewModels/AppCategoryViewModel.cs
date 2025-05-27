using Economy.Domain.Enums;
using Economy.Panel.UI.Models.CategoryViewModels.AppCategoryTranslationViewModels;

namespace Economy.Panel.UI.Models.CategoryViewModels
{
    public class AppCategoryViewModel
    {
        public int Id { get; set; }
        public ContentType ContentType { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<AppCategoryTranslationViewModel> Translations { get; set; } = new();
        public List<AppCategoryViewModel> SubCategories { get; set; } = new();
    }
}
