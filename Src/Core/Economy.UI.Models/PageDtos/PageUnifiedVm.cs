using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Models.PageDtos
{
    public sealed class PageUnifiedVm
    {
        public string Type { get; set; } = "";           // "list" | "detail"
        public string ContentType { get; set; } = "";    // "rooms" | "campaigns" | "page"
                                                         // ---- LIST alanları ----
        public List<PageListItemVm>? Items { get; set; }

        // ---- DETAIL alanları ----
        public int? Id { get; set; }
        public string? Lang { get; set; }
        public string? Slug { get; set; }
        public string? ParentTitle { get; set; }

        
        public string? ParentSlug { get; set; }
        public string? Url { get; set; }




        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public string? Image { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? OgImage { get; set; }

        public List<HreflangVm>? Hreflangs { get; set; }
        public List<MediaVm>? Gallery { get; set; }
        public List<BlockVm>? Blocks { get; set; }
    }

    public sealed class PageListItemVm
    {
        public int Id { get; set; }
        public string Slug { get; set; } = "";
        public string? ParentSlug { get; set; }
        public string? Url { get; set; }
        public string Title { get; set; } = "";
        public string? Summary { get; set; }
        public string? Image { get; set; }
        public DateTime? PublishAtUtc { get; set; }
        public bool IsActive { get; set; }
    }

    public sealed class HreflangVm
    {
        public string Lang { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Url { get; set; } = ""; // İstersen controller katmanında mutlak üret
    }

    public sealed class MediaVm
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
        public int SortOrder { get; set; }
        public string? Alt { get; set; }
        public string? Caption { get; set; }
    }

    public sealed class BlockVm
    {
        public int Id { get; set; }
        public string Template { get; set; } = ""; // "Hero" | "RichText" | "IncludeSnippet" | "RoomList" | "CampaignList" | "PageList" | "Gallery"
        public string Lang { get; set; } = "";
        public int SortOrder { get; set; }

        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public string? Image { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? JsonData { get; set; }

        // IncludeSnippet sonuçları
        public string? SnippetTitle { get; set; }
        public string? SnippetBody { get; set; }

        //// Kart listeleri
        public List<PageCardVm>? Rooms { get; set; }       // RoomList
        public List<CampaignCardVm>? Campaigns { get; set; } // CampaignList
        public List<PageCardVm>? Pages { get; set; }       // PageList
    }

    public record class PageCardVm
    {
        public int Id { get; set; }
        public string Slug { get; set; } = "";
        public string? ParentSlug { get; set; } = "";

        public string Title { get; set; } = "";
        public string? ParentTitle { get; set; } = "";

        public string? Summary { get; set; }
        public string? Image { get; set; }
        public string Url { get; set; } = ""; // "/tr/aile-odasi"
    }

    public record class CampaignCardVm : PageCardVm
    {
        public string? Badge { get; set; }
        public string? ValidFrom { get; set; }
        public string? ValidTo { get; set; }
        public decimal? PriceFrom { get; set; }
        public string? Currency { get; set; }
    }

}
