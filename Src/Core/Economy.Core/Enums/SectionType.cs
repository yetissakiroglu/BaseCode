using System.ComponentModel;

namespace Economy.Domain.Enums
{
    public enum SectionType
    {
        [Description("Html Bölüm")]
        HTML = 1,
        [Description("Manşet Bölümü")]
        Headline = 2,
        [Description("Öne Çıkanlar")]
        Featured = 3,
        [Description("Son Dakika Haberleri")]
        BreakingNews = 4,
        [Description("Yazarlar Bölümü")]
        Authors = 5,
        [Description("En Son Haberler")]
        LatestNews = 6,
        
    }
}
