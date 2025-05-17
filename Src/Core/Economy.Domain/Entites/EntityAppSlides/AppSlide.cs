using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntitySlides;

public class AppSlide : BaseEntity<int>
{
    public int Sequence { get; set; }
    public string? ThumbnailBase64 { get; set; }
    public string? ThumbnailMobilBase64 { get; set; }
    public virtual ICollection<AppSlideTranslation> Translations { get; set; } = new List<AppSlideTranslation>();
}
