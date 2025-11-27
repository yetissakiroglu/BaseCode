using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class DefRoomAttribute : BaseEntity<int>
    {
        public string Code { get; set; } = "";          // ROOM_TYPE, VIEW_TYPE, WIFI, BALCONY...
        public int DefRoomAttributeGroupId { get; set; }
        public DefRoomAttributeGroup DefRoomAttributeGroup { get; set; } = null!;

        public string InputType { get; set; } = "";     // Option, Bool, Number, Text
        public int SortOrder { get; set; }              // Sıralama

        public bool IsFilterable { get; set; }          // Filtre ekranında gözüksün mü?
        public bool IsRequired { get; set; }            // Her oda için zorunlu mu?
        public bool IsActive { get; set; } = true;

        public ICollection<DefRoomAttributeTranslation> Translations { get; set; }
            = new List<DefRoomAttributeTranslation>();

        public ICollection<DefRoomAttributeOption> Options { get; set; }
            = new List<DefRoomAttributeOption>();
    }
}
