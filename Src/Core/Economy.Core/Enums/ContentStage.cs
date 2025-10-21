using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Enums
{
    public enum ContentStage
    {
        Draft = 0,
        Published = 1,
        Archived = 2
    }
    public enum PageStage : byte // tinyint için byte iyi
    {
        [Display(Name = "Taslak")]
        Draft = 0,

        [Display(Name = "Yayında")]
        Published = 1,

        [Display(Name = "Arşivlendi")]
        Archived = 2
    }
}
