namespace MyHotelSite.Models;

public class SiteSettingDto
{
    public int AppId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string? LogoPath { get; set; }
    public string? FaviconPath { get; set; }
    public string? ShareImage { get; set; }
}

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

public class HomeViewModel
{
    public List<string> SliderImages { get; set; } = new();
    public string? SliderVideoUrl { get; set; }
    public List<Room> Rooms { get; set; } = new();
    public List<Campaign> Campaigns { get; set; } = new();
    public List<GalleryItem> Gallery { get; set; } = new();
}
