using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Models.SuperAdminViewModels
{
    public class EditRoleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Rol adı zorunludur.")]
        [Display(Name = "Rol Adı")]
        [StringLength(256)]
        public string Name { get; set; } = "";
    }
}
