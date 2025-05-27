using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityCategories;

namespace Economy.Domain.Entites.EntityAppCategories
{
    public class AppCategoryTranslation : BaseEntity<int>
    {
        public int AppCategoryId { get; set; }
        public AppCategory AppCategory { get; set; } = null!;

        public int AppLanguageId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        // URL artık sadece çeviri içinde
        public string Url { get; set; } = string.Empty;
    }
}
