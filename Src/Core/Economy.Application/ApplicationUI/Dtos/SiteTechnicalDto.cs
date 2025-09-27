namespace Economy.Application.ApplicationUI.Dtos
{

    public class SiteTechnicalDto
    {
        public string DefaultLanguage { get; set; } = "tr";
        public IReadOnlyList<string> SupportedLanguages { get; set; } = Array.Empty<string>();

        public string DomainName { get; set; }
        public bool ForceSSL { get; set; }
        public bool CdnEnabled { get; set; }
        public string? CdnBaseUrl { get; set; }
        public bool EnableDebugMode { get; set; }
        public bool MaintenanceModeEnabled { get; set; }
        public string? MaintenanceMessage { get; set; }
        public IReadOnlyList<string> MaintenanceAllowedIpList { get; set; } = Array.Empty<string>();
        public bool EnableOutputCache { get; set; }
        public int OutputCacheTtlSeconds { get; set; }
        public bool CookieBannerEnabled { get; set; }

    }
}
