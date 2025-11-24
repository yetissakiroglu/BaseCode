using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public class RoomAttributeOptionEditVm
    {
        public int Id { get; set; }

        public int DefRoomAttributeId { get; set; }     // Hangi özelliğe ait
        public string AttributeCode { get; set; } = ""; // Ekranda gösterim için

        public string Value { get; set; } = "";         // STANDARD, DELUXE, SEA...
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public string DisplayNameTr { get; set; } = ""; // Standart Oda
        public string DisplayNameEn { get; set; } = ""; // Standard Room
    }
}
