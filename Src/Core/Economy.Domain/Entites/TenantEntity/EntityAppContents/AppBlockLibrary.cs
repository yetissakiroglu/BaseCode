using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppContents
{
    public class AppBlockLibrary:BaseEntity<int>
    {
        [MaxLength(140)] public string Name { get; set; } = null!;
        public BlockType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Published;

        public string SharedJson { get; set; } = "{}";
        public ICollection<AppBlockLibraryTranslation> Translations { get; set; } = [];
    }
 
}
