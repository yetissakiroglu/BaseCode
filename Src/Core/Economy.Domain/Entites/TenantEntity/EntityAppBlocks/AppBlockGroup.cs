using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class AppBlockGroup : BaseEntity<int>
    {
        public bool IsActive { get; set; }
        public GroupTypes GroupType { get; set; } = GroupTypes.RoomView;
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public bool ShowSectionTitle { get; set; } = true;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public ICollection<AppBlockGroupTranslation> Translations { get; set; } = new List<AppBlockGroupTranslation>();
    }
}
