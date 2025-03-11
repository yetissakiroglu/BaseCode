using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Domain.Entites.EntityAppPages
{
    public class AppPageTranslation : BaseEntity<int>
    {
        public int AppLanguageId { get; set; }
        public virtual AppLanguage AppLanguage { get; set; } = null!;
        public required string Title { get; set; }
        public required string Url { get; set; }
        public string? Content { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
