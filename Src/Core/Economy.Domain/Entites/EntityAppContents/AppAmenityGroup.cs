using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityPages
{
    public class AppAmenityGroup:BaseEntity<int>
    {
        public int AppContentId { get; set; } 
        public AppContent AppContent { get; set; } = new AppContent();
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AppAmenity> AppAmenities { get; set; } = [];
    }
}
