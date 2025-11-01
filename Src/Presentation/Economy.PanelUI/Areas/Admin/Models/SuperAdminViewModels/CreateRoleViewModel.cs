using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Areas.Admin.Models.SuperAdminViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Rol adı zorunludur.")]
        [Display(Name = "Rol Adı")]
        [StringLength(256)]
        public string Name { get; set; } = "";
    }
}
