using Economy.Core.Extensions;
using Economy.Domain.BaseEntities;
using Economy.Domain.Models;

namespace Economy.Domain.Entites.EntityAppPages
{
    public class AppPage : BaseEntity<int>
    {
        public bool IsHomePage { get; set; }

        // ✅ Dil bağımlı çeviriler
        public ICollection<AppPageTranslation> Translations { get; set; } = new HashSet<AppPageTranslation>();

    }
}
