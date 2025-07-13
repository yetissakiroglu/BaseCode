using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Economy.Panel.UI.Models.UserViewModels
{
    public class AppUserCreateViewModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DisplayName("Adı")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DisplayName("Soyadı")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DisplayName("Bağlı Uygulama")]
        public int TenantId { get; set; }
        public List<UserAppListViewModel> Tenants { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [EmailAddress]
        [DisplayName("E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
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
