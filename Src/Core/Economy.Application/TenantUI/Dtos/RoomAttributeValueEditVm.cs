using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public class RoomAttributeValueEditVm
    {
        public int RoomId { get; set; }

        // Ekranda gruplanmış gösterim için
        public Dictionary<string, List<RoomAttributeItemVm>> GroupedAttributes { get; set; }
            = new();

        // POST sırasında gelen değerler için (name="Values[AttrId].Xxx" ile bağlı)
        public Dictionary<int, RoomAttributeValueInputVm> Values { get; set; }
            = new();
    }

    public class RoomAttributeItemVm
    {
        public int AttributeId { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = ""; // Option / Bool / Number / Text
        public string Group { get; set; } = "";

        // Option tipi
        public List<RoomAttributeOptionVm> Options { get; set; } = new();
        public int? SelectedOptionId { get; set; }

        // Mevcut değerler (ekrana doldurmak için)
        public bool? ValueBool { get; set; }
        public int? ValueInt { get; set; }
        public string? ValueText { get; set; }
        public List<RoomAttributeItemTranslationDto> Translations { get; set; }
                = new List<RoomAttributeItemTranslationDto>();

    }
    public class RoomAttributeItemTranslationDto
    {
        public int? Id { get; set; }
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string AppLanguageIcon { get; set; } = default!;
        public string? Text { get; set; }
    }





    public class RoomAttributeOptionVm
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = "";
    }

    // POST için input taşıyıcı (Values sözlüğünün içindeki tip)
    public class RoomAttributeValueInputVm
    {
        public int AttributeId { get; set; }
        public int? OptionId { get; set; }
        public bool? BoolValue { get; set; }
        public int? IntValue { get; set; }
        public string? TextValue { get; set; }
        public List<RoomAttributeItemTranslationDto> Translations { get; set; }
         = new List<RoomAttributeItemTranslationDto>();
    }
}
