using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class AppBlockGroup : BaseEntity<int>
    {
        public bool IsActive { get; set; }
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public ICollection<AppBlockGroupTranslation> Translations { get; set; } = new List<AppBlockGroupTranslation>();
        public ICollection<AppBlockGroupBlock> AppBlockGroupBlocks { get; set; } = [];
    }
}
