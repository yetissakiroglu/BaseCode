using System.ComponentModel;

namespace Economy.Domain.Enums
{
    public enum SectionType
    {
        [Description("Html Bölüm")]
        HTML = 1,
        [Description("Slider Bölümü")]
        Slide = 2,
        [Description("Odalar Bölümü")]
        Room = 3
    }
}
