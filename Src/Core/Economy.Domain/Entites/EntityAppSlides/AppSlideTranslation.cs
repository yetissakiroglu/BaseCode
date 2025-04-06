using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Domain.Entites.EntitySlides
{
	public class AppSlideTranslation : BaseEntity<int>
    {
        public int AppSlideId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? ButtonIcon { get; set; }
        public int AppLanguageId { get; set; }
        public virtual AppLanguage AppLanguage { get; set; } = null!;
    }
}
