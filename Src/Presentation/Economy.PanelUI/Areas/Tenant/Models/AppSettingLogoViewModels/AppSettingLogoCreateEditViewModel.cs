namespace Economy.Panel.UI.Areas.Tenant.Models.AppSettingLogoViewModels
{
    public class AppSettingLogoCreateEditViewModel
    {
        public int Id { get; set; }

        public string LogoPath { get; set; }
        public string? CroppedLogoBase64 { get; set; }

        public string? MobileLogoPath { get; set; }
        public string? CroppedMobileLogoBase64 { get; set; }

        public string? FaviconPath { get; set; }
        public string? CroppedFaviconBase64 { get; set; }

        public string? ShareImagePath { get; set; }
        public string? CroppedShareImageBase64 { get; set; }


        
    }
}
