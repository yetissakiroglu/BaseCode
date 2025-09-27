using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppTechnicalSetting : BaseEntity<int>
    {
        public string DomainName { get; set; }
        public bool ForceSSL { get; set; }
        public string? CdnBaseUrl { get; set; }
        public bool CdnEnabled { get; set; }
        public bool EnableDebugMode { get; set; }
        public bool MaintenanceModeEnabled { get; set; }
        public string? MaintenanceMessage { get; set; }
        public IReadOnlyList<string> MaintenanceAllowedIpList { get; set; } = Array.Empty<string>();
        public bool EnableOutputCache { get; set; }
        public int OutputCacheTtlSeconds { get; set; } = 300;
        public bool CookieBannerEnabled { get; set; }
    }

}
