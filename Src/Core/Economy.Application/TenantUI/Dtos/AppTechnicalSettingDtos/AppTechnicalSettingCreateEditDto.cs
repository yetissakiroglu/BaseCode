using Economy.Core.Extensions;
using System.ComponentModel;

namespace Economy.Application.TenantUI.Dtos.AppTechnicalSettingDtos
{
    public class AppTechnicalSettingCreateEditDto
    {
        public int Id { get; set; }

        [DisplayName("Domain Adı")]
        public string DomainName { get; set; }

        [DisplayName("SSL Zorunlu")]
        public bool ForceSSL { get; set; }

        [DisplayName("CDN Temel URL")]
        public string? CdnBaseUrl { get; set; }

        [DisplayName("CDN Aktif")]
        public bool CdnEnabled { get; set; }

        [DisplayName("Debug Modu Açık")]
        public bool EnableDebugMode { get; set; }

        [DisplayName("Bakım Modu Açık")]
        public bool MaintenanceModeEnabled { get; set; }

        [DisplayName("Bakım Mesajı")]
        public string? MaintenanceMessage { get; set; }

        [DisplayName("İzinli IP Listesi")]
        public IReadOnlyList<string> MaintenanceAllowedIpList { get; set; } = Array.Empty<string>();

        [DisplayName("Output Cache Açık")]
        public bool EnableOutputCache { get; set; }

        [DisplayName("Output Cache Süresi (sn)")]
        public int OutputCacheTtlSeconds { get; set; } = 300;

        [DisplayName("Çerez Uyarısı Aktif")]
        public bool CookieBannerEnabled { get; set; }
        // --- UI için Raw alan: GET => textarea’ya satır satır doldurur, SET => listeyi parse eder
        [DisplayName("İzinli IP Listesi (satır satır)")]
        public string MaintenanceAllowedIpListRaw
        {
            get => MaintenanceAllowedIpList is null || MaintenanceAllowedIpList.Count == 0
                ? string.Empty
                : string.Join(Environment.NewLine, MaintenanceAllowedIpList);
            set => MaintenanceAllowedIpList = value.ToIpList();
        }
    }
}
