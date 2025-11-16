using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Dtos
{
    public sealed record PageMediaDto(
       string Url,
       string? Alt,
       string? Caption,
       bool IsCover,
       int SortOrder
   );

    public sealed record BlockDto(
        int Id,
        BlockType Type,       // Mevcut enum’un: Hero, Text, ImageGallery, AmenityGroup
        string? Tag,
        byte Column,          // 1–12
        int SortOrder,
        object? Shared,       // Type’a göre: HeroSharedVm, GallerySharedVm, {}
        object? Localized     // Type’a göre: HeroLocVm, TextLocVm, AmenityLocVm, {}
    );

    public sealed record BlockGroupDto(
        int Id,
        BlockColumns Columns, // Mevcut enum’un: One/Two/Three/Four (veya 1–4)
        bool ShowTitle,
        bool ShowDescription,
        string? Title,
        string? Description,
        int SortOrder,
        IReadOnlyList<BlockDto> Blocks
    );

    public sealed record PageDetailDto(
        int Id,
        string Lang,
        string Slug,
        bool IsHomepage,
        string? Title,
        string? Summary,
        string? Body,
        string? MetaTitle,
        string? MetaDescription,
        string? CoverImageUrl,
        string? OgImageUrl,
        IReadOnlyList<PageMediaDto> Medias,
        IReadOnlyList<BlockGroupDto> Groups,
        IReadOnlyList<HreflangVm> Hreflangs,
        IReadOnlyList<BreadcrumbItemDto> Breadcrumbs
    );

    // --- JSON VM’leri (Shared/Localized) ---
    public class AmenityLocVm
    {
        public string GroupTitle { get; set; } = "Oda Olanakları";
        public List<string> Amenities { get; set; } = new List<string> { "Ücretsiz Wi-Fi", "Klima", "TV" };
    }

    public class GallerySharedVm
    {
        public string Mode { get; set; } = "grid"; // grid|masonry|slider
        public List<string> ImageUrls { get; set; } = new List<string>();
    }

    public class HeroSharedVm
    {
        public string VerticalAlign { get; set; } = "center"; // top|center|bottom
        public string BackgroundUrl { get; set; } = "/media/hero.jpg";
    }

    public class HeroLocVm
    {
        public string Heading { get; set; } = "";
        public string? SubHeading { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; } = "/";
    }

    public class TextLocVm
    {
        public string Heading { get; set; } = "Bölüm";
        public string BodyHtml { get; set; } = "<p>Metin…</p>";
    }

    public sealed class HreflangVm
    {
        public string Lang { get; set; } = "";
        public string Title { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Url { get; set; } = ""; // İstersen controller katmanında mutlak üret
    }


}
