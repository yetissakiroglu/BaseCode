using Economy.Domain.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace Economy.Domain.Entites.Identities
{
    public class AppUser : IdentityUser<int>, ISoftDelete, IHasId<int>,IHasAuditDates
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; } = false;
        public bool IsDeleted { get; set; }
        public string? JobTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt{ get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
