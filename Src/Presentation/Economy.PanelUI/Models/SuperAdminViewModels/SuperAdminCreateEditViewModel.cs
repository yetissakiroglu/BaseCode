using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Models.SuperAdminViewModels
{
    public class SuperAdminCreateEditViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DisplayName("Adı")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DisplayName("Soyadı")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
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

        [DisplayName("Varsayılan Yönetici Mi?")]
        public bool IsDefaultAdmin { get; set; }

        [DisplayName("Fotoğraf URL")]
        public string? PhotoUrl { get; set; }

        [DisplayName("Ünvan")]
        public string? JobTitle { get; set; }

        [DisplayName("2FA Aktif Mi?")]
        public bool TwoFactorEnabled { get; set; }

        [DisplayName("Hesap Kilitlenebilir Mi?")]
        public bool LockoutEnabled { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur.")]
        [DisplayName("Kullanıcı Rolü")]
        public string RoleName { get; set; }

        public List<SelectListItem>? RoleList { get; set; }
    }
}
