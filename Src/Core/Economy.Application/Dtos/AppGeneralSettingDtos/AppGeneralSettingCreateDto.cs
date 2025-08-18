using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppGeneralSettingDtos
{
    public class AppGeneralSettingCreateDto
    {
        public string SiteName { get; set; } = "";
        public string? Domain { get; set; }
        public string Theme { get; set; } = "light";
        public string? LogoUrl { get; set; }
        public string? MetaTitleSuffix { get; set; }
        public string? DefaultMetaDescription { get; set; }
    }
}
