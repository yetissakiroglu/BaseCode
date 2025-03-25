using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppPages
{
    public class AppPage : BaseEntity<int>
    {
        public bool IsHomePage { get; set; }

        // ✅ Dil bağımlı çeviriler
        public ICollection<AppPageTranslation> Translations { get; set; } = new HashSet<AppPageTranslation>();
        public ICollection<AppPageSection> AppPageSections { get; set; } = new HashSet<AppPageSection>();

    }
}
