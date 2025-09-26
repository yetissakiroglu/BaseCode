namespace MyHotelSite.Models;

public sealed class SeoViewModel
{
    public string MetaTitle { get; set; } = "";
    public string MetaDescription { get; set; } = "";
    public string? MetaKeywords { get; set; }
    public string CanonicalUrl { get; set; } = "";
    public string Robots { get; set; } = "index,follow";
    public string? ShareImage { get; set; }
    public string OgType { get; set; } = "website";
    public List<(string LangCode, string Url)> Hreflangs { get; set; } = new();
    public string? JsonLd { get; set; }
}

public sealed class SeoSeed
{
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string? ShareImage { get; init; }
    public string OgType { get; init; } = "website";
    public string? JsonLd { get; init; }
}
