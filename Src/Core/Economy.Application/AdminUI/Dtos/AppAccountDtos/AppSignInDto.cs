using System.ComponentModel.DataAnnotations;

namespace Economy.Application.AdminUI.Dtos.AppAccountDtos
{
    public class AppSignInDto
    {
        [Required(ErrorMessage = "Kullanýcý adý gereklidir.")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Þifre gereklidir.")]
        public string Password { get; set; } = default!;
        public bool RememberMe { get; set; } = false;
    }
}