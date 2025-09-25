using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppMenus;

namespace Economy.Domain.Entites.EntityMenuItems
{
    /// <summary>
    /// Menü öğelerini temsil eden sınıf.
    /// </summary>
    public class AppMenu : BaseEntity<int>
    {
        public string Location { get; set; } = "main";
        public int? PageId { get; set; }     // İçerik sayfası (Url XOR PageId)
        public MenuOpenTarget OpenTarget { get; set; } = MenuOpenTarget.SameTab;
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public bool IsExternal { get; set; }
        public int? ParentId { get; set; }
        public AppMenu? Parent { get; set; }
        public ICollection<AppMenu> Children { get; set; } = new List<AppMenu>();
        public virtual ICollection<AppMenuTranslation> Translations { get; set; } = new List<AppMenuTranslation>();
    }
}
