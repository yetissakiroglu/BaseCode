using Economy.Application.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Economy.Application.Helpers
{
    [HtmlTargetElement("img", Attributes = "cdn-src")]
    public class CdnImgTagHelper : TagHelper
    {
        private readonly ICdnUrlService _cdn;
        public CdnImgTagHelper(ICdnUrlService cdn) => _cdn = cdn;

        [HtmlAttributeName("cdn-src")] public string Src { get; set; } = "";
        [HtmlAttributeName("append-version")] public bool AppendVersion { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url = _cdn.Url(Src, AppendVersion);
            output.Attributes.SetAttribute("src", url);
            output.Attributes.RemoveAll("cdn-src");
            output.Attributes.RemoveAll("append-version");
        }
    }

    [HtmlTargetElement("link", Attributes = "cdn-href")]
    public class CdnLinkTagHelper : TagHelper
    {
        private readonly ICdnUrlService _cdn;
        public CdnLinkTagHelper(ICdnUrlService cdn) => _cdn = cdn;

        [HtmlAttributeName("cdn-href")] public string Href { get; set; } = "";
        [HtmlAttributeName("append-version")] public bool AppendVersion { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url = _cdn.Url(Href, AppendVersion);
            output.Attributes.SetAttribute("href", url);
            output.Attributes.RemoveAll("cdn-href");
            output.Attributes.RemoveAll("append-version");
        }
    }

    [HtmlTargetElement("script", Attributes = "cdn-src")]
    public class CdnScriptTagHelper : TagHelper
    {
        private readonly ICdnUrlService _cdn;
        public CdnScriptTagHelper(ICdnUrlService cdn) => _cdn = cdn;

        [HtmlAttributeName("cdn-src")] public string Src { get; set; } = "";
        [HtmlAttributeName("append-version")] public bool AppendVersion { get; set; } = false;
        [HtmlAttributeName("defer")] public bool Defer { get; set; } = false;
        [HtmlAttributeName("async")] public bool Async { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url = _cdn.Url(Src, AppendVersion);
            output.Attributes.SetAttribute("src", url);
            if (Defer) output.Attributes.SetAttribute("defer", null);
            if (Async) output.Attributes.SetAttribute("async", null);
            output.Attributes.RemoveAll("cdn-src");
            output.Attributes.RemoveAll("append-version");
        }
    }
}
