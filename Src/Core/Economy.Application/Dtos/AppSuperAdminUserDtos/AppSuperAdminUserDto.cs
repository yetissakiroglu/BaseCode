using Microsoft.AspNetCore.Identity;

namespace Economy.Application.Dtos.AppSuperAdminUserDtos
{
    public class AppSuperAdminUserDto : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public string? PhotoUrl { get; set; }
        public string? JobTitle { get; set; }
        public List<string> RolesName { get; set; }
    }
}
