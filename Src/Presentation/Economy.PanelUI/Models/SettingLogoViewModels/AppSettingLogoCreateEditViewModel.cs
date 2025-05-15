namespace Economy.Panel.UI.Models.SettingLogoViewModels
{
    public class AppSettingLogoCreateEditViewModel
    {
        public int Id { get; set; }

        public string LogoPath { get; set; } = string.Empty;
        public IFormFile? LogoFile { get; set; }
        public string? CroppedLogoBase64 { get; set; }

        public string? MobileLogoPath { get; set; }
        public IFormFile? MobileLogoFile { get; set; }
        public string? CroppedMobileLogoBase64 { get; set; }

        public string? FaviconPath { get; set; }
        public IFormFile? FaviconFile { get; set; }
        public string? CroppedFaviconBase64 { get; set; }
    }
}
