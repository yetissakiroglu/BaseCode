using System.ComponentModel;

namespace Economy.Panel.UI.Models.UserViewModels
{
    public class UserListViewModel
    {
        public int Id { get; set; }

        [DisplayName("Kullanıcı Adı")]
        public string FirstName { get; set; }
        [DisplayName("Kullanıcı Soyadı")]
        public string LastName { get; set; }
        [DisplayName("Bağlı Uygulama")]
        public string TenantName { get; set; }
        [DisplayName("Bağlı Uygulama")]
        public int TenantId { get; set; }

        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [DisplayName("E-posta")]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        public string Password { get; set; }

        [DisplayName("Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [DisplayName("E-posta Onaylandı mı?")]
        public bool EmailConfirmed { get; set; }

        [DisplayName("Telefon Onaylandı mı?")]
        public bool PhoneNumberConfirmed { get; set; }
        [DisplayName("Varsayılan Admin mi?")]
        public bool IsDefaultAdmin { get; set; }
    }
}
