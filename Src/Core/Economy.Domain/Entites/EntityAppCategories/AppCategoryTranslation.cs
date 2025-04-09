using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityCategories;

namespace Economy.Domain.Entites.EntityAppCategories
{
    public class AppCategoryTranslation : BaseEntity<int>
    {
        public int AppCategoryId { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;
    }
}
