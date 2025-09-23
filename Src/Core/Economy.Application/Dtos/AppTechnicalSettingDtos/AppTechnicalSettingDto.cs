using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppTechnicalSettingDtos
{
    public class AppTechnicalSettingDto
    {
        public int Id { get; set; }
        public bool IsSiteLive { get; set; }
        public bool ForceSSL { get; set; }
        public string DomainName { get; set; } = "";

        public bool EnableCDN { get; set; }
        public string? StaticFileUrl { get; set; }

        public bool EnableDebugMode { get; set; }
        public string? MaintenanceMessage { get; set; }

        public bool EnableCache { get; set; }

        public string? CustomCss { get; set; }
        public string? CustomJs { get; set; }

        public bool EnableMaintenanceIpWhitelist { get; set; }
        public string? AllowedIpAddresses { get; set; }

        public CustomScriptPlacement AllowedCustomScriptPlacements { get; set; } = CustomScriptPlacement.None;
        public string? CustomHeaderScripts { get; set; }
        public string? CustomFooterScripts { get; set; }

        public string? GoogleAnalyticsCode { get; set; }
        public string? FacebookPixelCode { get; set; }
    }
}
