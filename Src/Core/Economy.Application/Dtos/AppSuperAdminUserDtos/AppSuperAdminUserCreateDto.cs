using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Application.Dtos.AppSuperAdminUserDtos
{
    public class AppSuperAdminUserCreateDto 
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public string? PhotoUrl { get; set; }
        public string? JobTitle { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public List<string>? SelectedRoles { get; set; } = new();
    }
}
