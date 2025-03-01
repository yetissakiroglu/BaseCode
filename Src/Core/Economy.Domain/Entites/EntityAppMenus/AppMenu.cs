using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityMenuItems
{
    /// <summary>
    /// Menü öğelerini temsil eden sınıf.
    /// </summary>
    public class AppMenu : BaseEntity<int>
    {

        public string Title { get; set; }
        public string Slug { get; set; } // SEO uyumlu URL
        public bool IsExternal { get; set; }
        public int? ParentMenuId { get; set; }
        public AppMenu ParentMenu { get; set; }
        public ICollection<AppMenu> SubMenus { get; set; } = new List<AppMenu>();

        // URL formatında tam yol
        public string GetUrlPath()
        {
            return ParentMenu != null ? $"{ParentMenu.GetUrlPath()}/{Slug.ToLowerInvariant()}" : Slug.ToLowerInvariant();
        }

        // Breadcrumbs formatında tam yol
        public string GetBreadcrumbPath()
        {
            return ParentMenu != null ? $"{ParentMenu.GetBreadcrumbPath()} > {Title}" : Title;
        }
   
    }
}
