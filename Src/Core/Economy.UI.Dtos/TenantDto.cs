using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Dtos
{
    public class TenantDto
    {
        public string DefaultLanguage { get; set; } = "tr";
        public IReadOnlyList<string> SupportedLanguages { get; set; } = Array.Empty<string>();
        public IReadOnlyList<LanguagesDto> Languages { get; set; }
        public AppTheme ThemeKey { get; set; } = AppTheme.Classic;
        public TenantSettingsDto Settings { get; set; } = new();
    }
    public class LanguagesDto
    {
        public string Name { get; set; }
        public string Lang { get; set; } = "";
        public string Code { get; set; } = "";
        public bool IsRTL { get; set; } // Sağdan sola yazımı destekleyen diller
        public bool IsDefault { get; set; }
    }

    public class TenantSettingsDto
    {
        public string? CanonicalHost { get; set; }
        public string Domain { get; set; }
        public bool ForceHttps { get; set; }
        public bool ShowCookieBanner { get; set; }
        public bool EnableCdn { get; set; }
        public string? CdnBaseUrl { get; set; }
        public bool MaintenanceMode { get; set; }
        public string? MaintenanceMessage { get; set; }
        public bool EnableDebugMode { get; set; }
        public IReadOnlyList<string> MaintenanceAllowedIpList { get; set; } = Array.Empty<string>();
        public bool OutputCacheEnabled { get; set; }
        public int OutputCacheTtlSeconds { get; set; } = 300;
    }
}
