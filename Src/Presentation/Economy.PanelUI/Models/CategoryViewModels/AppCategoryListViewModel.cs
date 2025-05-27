using Economy.Domain.Enums;
using Economy.Panel.UI.Models.CategoryViewModels.AppCategoryTranslationViewModels;

namespace Economy.Panel.UI.Models.CategoryViewModels
{
    public class AppCategoryListViewModel
    {
        public int Id { get; set; }
        public ContentType ContentType { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<AppCategoryTranslationViewModel> Translations { get; set; } = new();
    }
}
