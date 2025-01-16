using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntitySlides;

public class AppSlide : BaseEntity<int>
{
    public int AppSectionId { get; set; }
    public int Sequence { get; set; }
    /// <summary>
    /// Bu slide öğesine ait çevirileri içerir.
    /// </summary>
    public virtual ICollection<AppSlideTranslation> AppSlideTranslations { get; set; } = [];
  
}
