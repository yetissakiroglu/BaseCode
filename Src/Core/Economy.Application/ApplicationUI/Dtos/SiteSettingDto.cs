namespace Economy.Application.ApplicationUI.Dtos
{
    public class SiteSettingDto
    {
        public int AppId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? LogoPath { get; set; }
        public string? FaviconPath { get; set; }
        public string? ShareImage { get; set; }
    }




}
