using Microsoft.AspNetCore.Identity;

namespace Economy.Base.Application.Dtos.BaseModels
{
    public class AppUserDto : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public int TenantId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? JobTitle { get; set; }
    }
}
