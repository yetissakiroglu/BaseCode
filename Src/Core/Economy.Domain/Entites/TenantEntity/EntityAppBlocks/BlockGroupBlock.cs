using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class BlockGroupBlock:BaseEntity<int>
    {
        public int BlockGroupId { get; set; }
        public BlockGroup BlockGroup { get; set; }
        public BlockType Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Draft;

        // Dilden bağımsız alanlar (layout, görsel url, kolon, vb.)
        public string SharedJson { get; set; } = "{}";

        public ICollection<BlockGroupBlockTranslation> Translations { get; set; } = [];
    }
}
