using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityAppSettings;

namespace Economy.Domain.Entites.EntityMenuItems
{
    /// <summary>
    /// Menü öğelerini temsil eden sınıf.
    /// </summary>
    public class AppMenu : BaseEntity<int>
    {
        public bool IsExternal { get; set; }
        public int? ParentMenuId { get; set; }
        public AppMenu ParentMenu { get; set; }
        public ICollection<AppMenu> SubMenus { get; set; } = new List<AppMenu>();

        //// URL formatında tam yol
        //public string GetUrlPath()
        //{
        //    return ParentMenu != null ? $"{ParentMenu.GetUrlPath()}/{Slug.ToLowerInvariant()}" : Slug.ToLowerInvariant();
        //}

        //// Breadcrumbs formatında tam yol
        //public string GetBreadcrumbPath()
        //{
        //    return ParentMenu != null ? $"{ParentMenu.GetBreadcrumbPath()} > {Title}" : Title;
        //}

        public virtual ICollection<AppMenuTranslation> Translations { get; set; } = new List<AppMenuTranslation>();


    }
}
