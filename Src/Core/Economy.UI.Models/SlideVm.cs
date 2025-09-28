using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Models
{

    public class SlideVm
    {
        public int Id { get; set; }
        public string Image { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ButtonText { get; set; }

        public bool IsExternal { get; set; }
        public string? Url { get; set; }

        public SlideTargetEnum Target { get; set; } = SlideTargetEnum.Self;
    }

}
