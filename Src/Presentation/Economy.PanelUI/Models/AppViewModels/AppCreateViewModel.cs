using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Models.AppViewModels
{
    public class AppCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [DisplayName("Otel Adı")]
        public string HotelName { get; set; }

        [Required]
        [MaxLength(200)]
        [DisplayName("Sunucu Adı")]
        public string ServerName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Veritabanı Adı")]
        public string DatabaseName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Şifre Var mı?")]
        public bool IsPassword { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Şifre")]
        public string Password { get; set; }

        [MaxLength(250)]
        [DisplayName("Domain")]
        public string Domain { get; set; }
    }
}
