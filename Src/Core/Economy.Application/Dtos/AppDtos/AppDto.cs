using Economy.Core.Enums;
using Economy.Core.Extensions;
using Economy.Core.Helpers;
using System.ComponentModel;

namespace Economy.Panel.Application.Dtos.AppDtos
{
    public class AppDto
    {
        public int Id { get; set; }

        [DisplayName("Otel Adı")]
        public string HotelName { get; set; }

        [DisplayName("Sunucu Adı")]
        public string ServerName { get; set; }

        [DisplayName("Veritabanı Adı")]
        public string DatabaseName { get; set; }

        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [DisplayName("Şifre Var Mı?")]
        public bool IsPassword { get; set; }

        [DisplayName("Şifre Durumu")]
        public string PasswordStatus => IsPassword.GetYesNoText();

        [DisplayName("Şifre")]
        public string Password { get; set; }

        [DisplayName("Domain")]
        public string Domain { get; set; }

        [DisplayName("Bağlantı Cümlesi")]
        public string ConnectionString { get; set; }

        public AppManagerDto AppManager { get; set; }


        public AppAccessMode AccessMode { get; set; }
        public AppTheme Theme { get; set; }
        public string? ApiKey { get; set; }   // yeni alan

    }

    public class AppManagerDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }


}
