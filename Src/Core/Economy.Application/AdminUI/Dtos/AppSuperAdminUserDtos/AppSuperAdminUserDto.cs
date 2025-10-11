using Microsoft.AspNetCore.Identity;

namespace Economy.Application.AdminUI.Dtos.AppSuperAdminUserDtos
{
    public class AppSuperAdminUserDto : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public string? JobTitle { get; set; }
        public string RolesName { get; set; }
    }
}
