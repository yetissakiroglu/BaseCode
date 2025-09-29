namespace Economy.Web.Demo1.Models
{


    public sealed class SeoSeed
    {
        public string Title { get; init; } = "";
        public string Description { get; init; } = "";
        public string? ShareImage { get; init; }
        public string OgType { get; init; } = "website";
        public string? JsonLd { get; init; }
    }

}
