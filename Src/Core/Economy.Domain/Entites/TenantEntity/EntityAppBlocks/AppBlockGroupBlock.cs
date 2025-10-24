using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{

    public class AppBlockGroupBlock : BaseEntity<int>
    {
        public int AppBlockGroupId { get; set; }
        public AppBlockGroup AppBlockGroup { get; set; } = default!;

        public int AppBlockId { get; set; }
        public AppBlock Block { get; set; } = default!;

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Grid yerleşimi için zorunlu 1–12
        public byte Column { get; set; } = 12;
    }
}
