using System.ComponentModel.DataAnnotations;

namespace Economy.Application.ApplicationUI.Dtos
{
    public class SiteSettingDto
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
