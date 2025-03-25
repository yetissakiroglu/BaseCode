using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntitySlides;

public class AppSlide : BaseEntity<int>
{
    public int AppSectionId { get; set; }
    public int Sequence { get; set; }
    public virtual ICollection<AppSlideTranslation> Translations { get; set; } = new List<AppSlideTranslation>();

}
