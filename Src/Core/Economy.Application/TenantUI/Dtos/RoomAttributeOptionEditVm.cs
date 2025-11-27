using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public class RoomAttributeOptionEditVm
    {
        public int? Id { get; set; }
        public int DefRoomAttributeId { get; set; }     // Hangi özelliğe ait
        public string AttributeCode { get; set; } = ""; // Ekranda gösterim için
        public string Value { get; set; } = "";         // STANDARD, DELUXE, SEA...
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public List<RoomAttributeOptionTranslationDto> Translations { get; set; }
         = new List<RoomAttributeOptionTranslationDto>();
    }

    public class RoomAttributeOptionTranslationDto
    {
        public int? Id { get; set; }
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string AppLanguageIcon { get; set; } = default!;
        public string? DisplayName { get; set; } = "";   // "Deluxe Oda" / "Deluxe Room"
    }


    public class RoomAttributeOptionListVm
    {
        public int Id { get; set; }
        public int DefRoomAttributeId { get; set; }     // Hangi özelliğe ait
        public string AttributeCode { get; set; } = ""; // Ekranda gösterim için
        public string Value { get; set; } = "";         // STANDARD, DELUXE, SEA...
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public string DisplayName { get; set; } = "";   // "Deluxe Oda" / "Deluxe Room"
    }



}
