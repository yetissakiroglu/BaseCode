using Economy.Application.Dtos.AppUserDtos;
using Microsoft.AspNetCore.Identity;

namespace Economy.Base.Application.Dtos.BaseModels
{
    public class AppUserDto : IdentityUser<int>
    {
        public AppUserDto()
        {
            Roles = new List<string>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public int TenantId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? JobTitle { get; set; }

        public List<string> Roles { get; set; }
    }
}
