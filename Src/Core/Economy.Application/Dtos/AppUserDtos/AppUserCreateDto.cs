using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Economy.Base.Application.Dtos.BaseModels
{
    public class AppUserCreateDto : IdentityUser<int>
    {
        [Required]
        [DisplayName("Adı")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Soyadı")]
        public string LastName { get; set; }
        //[Required]
        //[DisplayName("Bağlı Uygulama")]
        //public int TenantId { get; set; }

        [Required]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("E-posta")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Şifre")]
        public string Password { get; set; }

        [DisplayName("Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [DisplayName("E-posta Onaylandı mı?")]
        public bool EmailConfirmed { get; set; }

        [DisplayName("Telefon Onaylandı mı?")]
        public bool PhoneNumberConfirmed { get; set; }

    }
}

