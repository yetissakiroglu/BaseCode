using Economy.Domain.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace Economy.Domain.Entites.Identities
{
    public class AppUser : IdentityUser<int>, ISoftDelete, IHasId<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; } = false;
        public bool IsDeleted { get; set; }
        //public int TenantId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? JobTitle { get; set; }
    }
}
