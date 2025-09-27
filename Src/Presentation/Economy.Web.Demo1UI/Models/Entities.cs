namespace MyHotelSite.Models;


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
