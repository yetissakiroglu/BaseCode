using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntitySlides;

public class AppSlide : BaseEntity<int>
{
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; } = 1;

    public string ImagePath { get; set; } = string.Empty; // /uploads/slider/1.jpg
    public bool IsExternal { get; set; }                  // link dış site mi?
    public string? LinkUrl { get; set; }

    public SlideTargetEnum OpenTarget { get; set; } = SlideTargetEnum.Self;
    public virtual ICollection<AppSlideTranslation> Translations { get; set; } = new List<AppSlideTranslation>();
}
