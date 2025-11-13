using Economy.Core.Dtos.Custom;
using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Areas.Tenant.Models
{

    // --- HERO ---
    // Shared: backgroundUrl, verticalAlign
    public class HeroSharedVm
    {
        [Required] public string VerticalAlign { get; set; } = "center"; // top|center|bottom
        [Required] public string BackgroundUrl { get; set; } = "/media/hero.jpg";
    }

    // Localized: heading, subHeading, buttonText, buttonUrl
    public class HeroLocVm
    {
        [Required, StringLength(60, MinimumLength = 4)]
        public string Heading { get; set; } = "";

        [StringLength(140)]
        public string? SubHeading { get; set; }

        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; } = "/";
    }


    // --- TEXT ---
    // Shared: {}
    // Localized: heading, bodyHtml
    public class TextLocVm
    {
        [Required, StringLength(60, MinimumLength = 2)]
        public string Heading { get; set; } = "Bölüm";

        public string BodyHtml { get; set; } = "<p>Metin…</p>";
    }


    // --- IMAGE GALLERY ---
    // Shared: mode, imageUrls ([])
    public class GallerySharedVm
    {
        [Required] public string Mode { get; set; } = "grid"; // grid|masonry|slider
        public List<string> ImageUrls { get; set; } = new();
    }

    // Localized: {} (boş sözleşme)
    public class GalleryLocVm
    {
        // Faz-1 gereği burada alan yok
    }


    // --- AMENITY GROUP ---
    // Shared: {} (yok)
    // Localized: groupTitle, amenities ([])
    public class AmenityLocVm
    {
        [Required, StringLength(80, MinimumLength = 2)]
        public string GroupTitle { get; set; } = "Oda Olanakları";

        public List<string> Amenities { get; set; } = new()
    {
        "Ücretsiz Wi-Fi",
        "Klima",
        "TV"
    };
    }


}
