using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public class RoomAttributeEditVm
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";          // ROOM_TYPE, VIEW_TYPE...
        public string Group { get; set; } = "";         // Summary, Amenity, Filter...
        public string InputType { get; set; } = "";     // Option, Bool, Number, Text

        public int SortOrder { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; } = true;

        // TR / EN isimleri (basit kullanım)
        public string NameTr { get; set; } = "";
        public string NameEn { get; set; } = "";
    }

}
