using Economy.Domain.BaseEntities;
using Economy.Domain.Models;

namespace Economy.Domain.Entites.EntityAppPages
{
    public class AppPage : BaseEntity<int>
    {
        public bool IsHomePage { get; set; }

        // ✅ Dil bağımlı çeviriler
        public ICollection<AppPageTranslation> Translations { get; set; } = new HashSet<AppPageTranslation>();
        public ICollection<AppPageSection> AppPageSections { get; set; } = new HashSet<AppPageSection>();

        public List<BreadcrumbDto> GetBreadcrumbs()
        {
            // Eğer hiyerarşik yapı olmayacaksa breadcrumb sadece mevcut sayfayı içermeli.
            return new List<BreadcrumbDto>
        {
            new BreadcrumbDto
            {
                Name = this.Translations?.FirstOrDefault().Title,
                Url = "/" + this.GetUrlPath()
            }
        };
        }

        // URL formatında tam yol (Hiyerarşi kaldırıldığı için direkt slug döndürüyor)
        public string GetUrlPath()
        {
            return Translations?.FirstOrDefault().Url.ToLowerInvariant();
        }

    }
}
