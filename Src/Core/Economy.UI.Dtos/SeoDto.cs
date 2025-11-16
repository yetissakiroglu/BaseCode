using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Dtos
{
    public sealed class SeoDto
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
