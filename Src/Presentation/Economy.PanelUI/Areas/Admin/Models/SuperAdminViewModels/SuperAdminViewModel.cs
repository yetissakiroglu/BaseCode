using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Areas.Admin.Models.SuperAdminViewModels
{
    public class SuperAdminViewModel
    {
        [Display(Name = "Kullanıcı ID")]
        public int UserId { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Varsayılan Yönetici")]
        public bool IsDefaultAdmin { get; set; }

        [Display(Name = "Silinmiş Mi?")]
        public bool IsDeleted { get; set; }
      
        [Display(Name = "Görev Ünvanı")]
        public string? JobTitle { get; set; }

        [Display(Name = "E-Posta")]
        public string? Email { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "E-Posta Doğrulandı")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Telefon Doğrulandı")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Yetki")]
        public string RolesName { get; set; }
    }
}
