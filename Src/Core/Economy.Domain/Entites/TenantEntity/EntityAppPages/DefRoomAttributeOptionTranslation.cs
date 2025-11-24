using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class DefRoomAttributeOptionTranslation : BaseEntity<int>
    {
        public int DefRoomAttributeOptionId { get; set; }
        public DefRoomAttributeOption DefRoomAttributeOption { get; set; } = null!;

        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        public string DisplayName { get; set; } = "";   // "Deluxe Oda" / "Deluxe Room"
    }
}
