using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class AppBlockGroupBlock:BaseEntity<int>
    {
        public string? Tag { get; set; }
        public int AppBlockGroupId { get; set; }
        public AppBlockGroup AppBlockGroup { get; set; } = null;
        public BlockType Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Draft;
        public string SharedJson { get; set; } = "{}";
        public ICollection<AppBlockGroupBlockTranslation> Translations { get; set; } = [];
    }
}
