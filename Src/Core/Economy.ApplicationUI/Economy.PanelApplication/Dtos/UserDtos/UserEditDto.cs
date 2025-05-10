using Microsoft.AspNetCore.Identity;

namespace Economy.Panel.Application.Dtos.UserDtos
{
    public class UserEditDto : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public int TenantId { get; set; }

    }
}
