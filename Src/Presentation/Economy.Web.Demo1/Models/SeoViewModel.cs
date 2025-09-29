namespace Economy.Web.Demo1.Models
{
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
}
