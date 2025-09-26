namespace Economy.Application.ApplicationUI.Dtos
{

    public class SiteTechnicalDto
    {
        public int AppId { get; set; }
        public string DefaultLanguage { get; set; } = "tr";
        public IReadOnlyList<string> SupportedLanguages { get; set; } = Array.Empty<string>();
        public string? CdnBaseUrl { get; set; }
        public bool CdnEnabled { get; set; }
        public bool EnableOutputCache { get; set; }
        public int OutputCacheTtlSeconds { get; set; }
        public bool MaintenanceModeEnabled { get; set; }
        public IReadOnlyList<string> MaintenanceAllowedIpList { get; set; } = Array.Empty<string>();
        public bool CookieBannerEnabled { get; set; }
        public string? GoogleTagManagerId { get; set; }
        public string? GoogleAnalyticsId { get; set; }
        public IReadOnlyDictionary<string, string>? HreflangDomainMap { get; set; }
    }
}
