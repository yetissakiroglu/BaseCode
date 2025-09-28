using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Domain.Entites.EntitySlides
{
	public class AppSlideTranslation : BaseEntity<int>
    {
        public int AppSlideId { get; set; }
        public virtual AppSlide AppSlide { get; set; } = default!;

        public int AppLanguageId { get; set; }
        public virtual AppLanguage Language { get; set; } = default!;

        public string Title { get; set; } = string.Empty;     // Başlık
        public string? Description { get; set; }              // Açıklama
        public string? ButtonText { get; set; }               // Buton metni (ops.)
    }
}
