using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Dtos
{
    public class SiteMetaDto
    {
        public string? SiteTitle { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? MetaTitle { get; set; } = "";
        public string? MetaSlogan { get; set; } = "";
        public string? MetaDescription { get; set; } = "";
        public string? LogoPath { get; set; }
        public string? FaviconPath { get; set; }
        public string? ShareImagePath { get; set; }
    }
}
