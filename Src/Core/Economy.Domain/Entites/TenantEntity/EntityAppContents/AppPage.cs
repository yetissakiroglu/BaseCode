using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppContents
{
    public class AppPage:BaseEntity<int>
    {
        [MaxLength(160)] public string Slug { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Draft;

        public ICollection<AppPageTranslation> Translations { get; set; } = [];
        public ICollection<AppPageBlock> AppPageBlocks { get; set; } = [];
    }
}
