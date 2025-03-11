using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Domain.Entites.EntityAppMenus
{
    public  class AppMenuTranslation : BaseEntity<int>
    {
        public int AppLanguageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; } // SEO uyumlu URL

        public virtual AppLanguage AppLanguage { get; set; } = null!;
    }
}
