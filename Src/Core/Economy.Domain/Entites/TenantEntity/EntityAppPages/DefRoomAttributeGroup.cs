using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class DefRoomAttributeGroup : BaseEntity<int>
    {
        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<DefRoomAttributeGroupTranslation> Translations { get; set; }
            = new List<DefRoomAttributeGroupTranslation>();

        public ICollection<DefRoomAttribute> Attributes { get; set; }
            = new List<DefRoomAttribute>();
    }
}
