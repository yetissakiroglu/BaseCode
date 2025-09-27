using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Models
{
    public class SiteConfigResponse
    {
        public SiteSettingDto? Setting { get; set; }
        public SiteTechnicalDto? Technical { get; set; }
    }
}
