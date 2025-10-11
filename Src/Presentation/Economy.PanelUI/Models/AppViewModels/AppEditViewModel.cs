using Economy.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Models.AppViewModels
{
    public class AppEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Otel adı gereklidir.")]
        [MaxLength(200)]
        [DisplayName("Uygulama Adı")]
        public string HotelName { get; set; }

        [Required(ErrorMessage = "Sunucu adı gereklidir.")]
        [MaxLength(200)]
        [DisplayName("Sunucu Adı")]
        public string ServerName { get; set; }

        [Required(ErrorMessage = "Veritabanı adı gereklidir.")]
        [MaxLength(100)]
        [DisplayName("Veritabanı Adı")]
        public string DatabaseName { get; set; }

        [DisplayName("Kullanıcı Adı")]
        public string? UserName { get; set; }

        [DisplayName("Şifre Var mı?")]
        public bool IsPassword { get; set; }

        [DisplayName("Şifre")]
        public string? Password { get; set; }

        [MaxLength(250)]
        [DisplayName("Domain")]
        public string Domain { get; set; }

        // Yeni alanlar
        public AppAccessMode AccessMode { get; set; }
        public AppTheme Theme { get; set; }
        public string? ApiKey { get; set; }   // yeni alan
    }
}
