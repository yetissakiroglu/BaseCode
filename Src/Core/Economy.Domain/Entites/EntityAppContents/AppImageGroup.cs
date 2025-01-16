using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityPages
{
    public class AppImageGroup : BaseEntity<int>
    {
        public int AppContentId { get; set; } // Odanın ID'si
        public AppContent AppContent { get; set; } = new AppContent();
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<AppImage> AppImages { get; set; } = [];

    }
}
