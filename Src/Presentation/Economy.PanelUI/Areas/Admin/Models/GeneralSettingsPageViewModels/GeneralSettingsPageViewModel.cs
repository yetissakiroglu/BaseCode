using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Areas.Admin.Models.GeneralSettingsPageViewModels
{
    public class GeneralSettingsPageViewModel
    {
        public int? Id { get; set; } // null => ilk kez oluşturulacak

        [Required, Display(Name = "Site Adı"), StringLength(200)]
        public string SiteName { get; set; } = "";

        [Display(Name = "Domain"), StringLength(200)]
        public string? Domain { get; set; }

        [Required, Display(Name = "Tema"), StringLength(20)]
        public string Theme { get; set; } = "light"; // light | dark

        [Display(Name = "Logo URL"), StringLength(300)]
        public string? LogoUrl { get; set; }

        [Display(Name = "Logo Yükle (opsiyonel)")]
        public IFormFile? LogoFile { get; set; }

        [Display(Name = "Meta Title Soneki"), StringLength(120)]
        public string? MetaTitleSuffix { get; set; }

        [Display(Name = "Varsayılan Meta Description"), StringLength(300)]
        public string? DefaultMetaDescription { get; set; }

      
    }
}
