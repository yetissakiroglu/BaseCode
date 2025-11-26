using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public class RoomAttributeListVm
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";          // ROOM_TYPE, VIEW_TYPE...
        public string Group { get; set; } = "";         // Summary, Amenity, Filter...
        public string InputType { get; set; } = "";     // Option, Bool, Number, Text
        public int SortOrder { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public string? Description { get; set; }

    }
    public class RoomAttributeEditVm
    {
        public int? Id { get; set; }
        public string Code { get; set; } = "";          // ROOM_TYPE, VIEW_TYPE...
        public string Group { get; set; } = "";         // Summary, Amenity, Filter...
        public string InputType { get; set; } = "";     // Option, Bool, Number, Text

        public int SortOrder { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; } = true;

        public List<RoomAttributeTranslationDto> Translations { get; set; }
            = new List<RoomAttributeTranslationDto>();

    }
    public class RoomAttributeTranslationDto
    {
        public int? Id { get; set; }
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string AppLanguageIcon { get; set; } = default!;
        public string Name { get; set; }
        public string? Description { get; set; }
    }






}
