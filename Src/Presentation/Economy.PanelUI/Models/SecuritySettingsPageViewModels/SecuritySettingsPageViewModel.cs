using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Models.SecuritySettingsPageViewModels
{
    public class SecuritySettingsPageViewModel
    {
        public int? Id { get; set; }

        // Password policy
        [Display(Name = "Minimum Parola Uzunluğu")]
        [Range(4, 128)]
        public int PasswordRequiredLength { get; set; } = 6;

        [Display(Name = "Rakam Zorunlu")]
        public bool PasswordRequireDigit { get; set; } = true;

        [Display(Name = "Küçük Harf Zorunlu")]
        public bool PasswordRequireLowercase { get; set; } = true;

        [Display(Name = "Büyük Harf Zorunlu")]
        public bool PasswordRequireUppercase { get; set; } = false;

        [Display(Name = "Alfasayısal Olmayan Karakter Zorunlu")]
        public bool PasswordRequireNonAlphanumeric { get; set; } = false;

        // Lockout policy
        [Display(Name = "Kilit Süresi (dakika)")]
        [Range(1, 1440)]
        public int LockoutTimeSpanMinutes { get; set; } = 30;

        [Display(Name = "Maks. Başarısız Giriş")]
        [Range(1, 20)]
        public int LockoutMaxFailedAccessAttempts { get; set; } = 5;

        [Display(Name = "Yeni Kullanıcılarda Lockout Açık")]
        public bool LockoutAllowedForNewUsers { get; set; } = true;

        // SignIn policy
        [Display(Name = "E-posta Doğrulaması Zorunlu")]
        public bool SignInRequireConfirmedEmail { get; set; } = false;

        [Display(Name = "Telefon Doğrulaması Zorunlu")]
        public bool SignInRequireConfirmedPhoneNumber { get; set; } = false;

        // 2FA
        [Display(Name = "Tüm Kullanıcılarda 2FA Zorunlu")]
        public bool TwoFactorRequired { get; set; } = false;
    }
}
