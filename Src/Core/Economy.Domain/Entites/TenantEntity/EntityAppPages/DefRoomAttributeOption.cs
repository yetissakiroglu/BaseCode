using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class DefRoomAttributeOption : BaseEntity<int>
    {
        public int DefRoomAttributeId { get; set; }
        public DefRoomAttribute DefRoomAttribute { get; set; } = null!;

        public string Value { get; set; } = "";         // STANDARD, DELUXE, SEA, KING_BED...
        public int SortOrder { get; set; }              // Sıralama
        public bool IsActive { get; set; } = true;

        public ICollection<DefRoomAttributeOptionTranslation> Translations { get; set; }
            = new List<DefRoomAttributeOptionTranslation>();
    }
}
