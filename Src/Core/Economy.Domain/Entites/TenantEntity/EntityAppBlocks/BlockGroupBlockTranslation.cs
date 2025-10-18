using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppBlocks
{
    public class BlockGroupBlockTranslation : BaseEntity<int>
    {

        public int BlockGroupBlockId { get; set; }
        public BlockGroupBlock BlockGroupBlock { get; set; }
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        // Dile bağlı metinler (hero başlık, açıklamalar, vb.)
        public string LocalizedJson { get; set; } = "{}";
    }
}
