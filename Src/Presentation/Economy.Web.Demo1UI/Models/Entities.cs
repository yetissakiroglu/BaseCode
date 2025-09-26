namespace MyHotelSite.Models;

public class AppSetting
{
    public int AppId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string? LogoPath { get; set; }
    public string? FaviconPath { get; set; }
    public string? ShareImage { get; set; }
}

public class AppSettingTechnical
{
    public int AppId { get; set; }
    public string DefaultLanguage { get; set; } = "tr";
    public List<string> SupportedLanguages { get; set; } = new() { "tr", "en" };
    public string? CdnBaseUrl { get; set; }
    public bool CdnEnabled { get; set; } = true;
    public bool EnableOutputCache { get; set; } = true;
    public int OutputCacheTtlSeconds { get; set; } = 300;
    public bool EnableCompression { get; set; } = true;
    public bool DebugEnableCdn { get; set; } = false;
    public bool MaintenanceModeEnabled { get; set; } = false;
    public List<string> MaintenanceAllowedIpList { get; set; } = new();
    public bool CookieBannerEnabled { get; set; } = true;

    // Analytics
    public string? GoogleTagManagerId { get; set; }
    public string? GoogleAnalyticsId { get; set; }

    // Multi-domain hreflang
    public Dictionary<string, string> HreflangDomainMap { get; set; } = new();
}

// Content entities
public class Page
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Lang { get; set; } = "tr";
    public string SectionKey { get; set; } = "page";
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? HeroImage { get; set; }
    public string? ContentHtml { get; set; }
}

public class Room
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Lang { get; set; } = "tr";
    public string HotelName { get; set; } = "";
    public string RoomName { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? ShortDescription { get; set; }
    public string? MainImageUrl { get; set; }
    public List<string> Gallery { get; set; } = new();
    public decimal? Price { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public class Campaign
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Lang { get; set; } = "tr";
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? ShortDescription { get; set; }
    public string? HeroImage { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidThrough { get; set; }
}

public class GalleryItem
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Lang { get; set; } = "tr";
    public string Url { get; set; } = "";
    public string? Caption { get; set; }
    public bool IsFeatured { get; set; } = false;
    public string Slug { get; set; } = "";
}

// Localization key-value
public class LocalizationString
{
    public string Key { get; set; } = "";
    public string Lang { get; set; } = "tr";
    public string Value { get; set; } = "";
}

// Menu
public class MenuItem
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Lang { get; set; } = "tr";
    public int? ParentId { get; set; }
    public string Title { get; set; } = "";
    public bool IsExternal { get; set; } = false;
    public string? ExternalUrl { get; set; }
    public int? PageId { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}
