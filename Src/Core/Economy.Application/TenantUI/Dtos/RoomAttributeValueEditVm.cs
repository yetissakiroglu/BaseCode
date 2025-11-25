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

        public Dictionary<string, List<RoomAttributeItemVm>> GroupedAttributes { get; set; }
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

        // Bool tipi
        public bool? ValueBool { get; set; }

        // Number tipi
        public int? ValueInt { get; set; }

        // Text tipi
        public string? ValueText { get; set; }
    }

    public class RoomAttributeOptionVm
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = "";
    }
}
