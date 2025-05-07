using Economy.Core.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        public string PasswordStatus => DisplayHelper.GetYesNoText(IsPassword);

        [DisplayName("Şifre")]
        public string Password { get; set; }

        [DisplayName("Domain")]
        public string Domain { get; set; }

        [DisplayName("Bağlantı Cümlesi")]
        public string ConnectionString { get; set; }

    }
}
