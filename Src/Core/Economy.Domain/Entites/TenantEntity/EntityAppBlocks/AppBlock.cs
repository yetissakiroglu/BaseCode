using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class AppBlock : BaseEntity<int>
    {
        public string? Tag { get; set; }
        public BlockType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public string SharedJson { get; set; } = "{}";
        public ICollection<AppBlockTranslation> Translations { get; set; } = [];
    }
}
