using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityPages
{
    public class AppAmenity : BaseEntity<int>
    {
        public int AppAmenityGroupId { get; set; }
        public AppAmenityGroup AppAmenityGroup { get; set; } = new AppAmenityGroup();
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
