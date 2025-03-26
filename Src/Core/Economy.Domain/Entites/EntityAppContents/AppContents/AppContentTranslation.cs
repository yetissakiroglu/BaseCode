using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Domain.Entites.EntityAppContents.AppContents
{
    public class AppContentTranslation : BaseEntity<int>
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string Url { get; set; }

        // SEO alanları
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int AppLanguageId { get; set; }
        public virtual AppLanguage AppLanguage { get; set; } = null!;

    }
}
