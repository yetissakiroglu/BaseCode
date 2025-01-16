using Economy.Domain.BaseEntities;
using Economy.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Economy.Domain.Entites.Identities
{
	public class AppUser : IdentityUser, ISoftDelete, IHasId<string>
	{
		public string? FullName { get; set; }
        public string? JobTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? Avatar { get; set; }
        public bool IsDefaultAdmin { get; set; } = false;
        public RegistrationType RegistrationType { get; set; } = RegistrationType.RegistrationBySelf;
        public bool? IsOnline { get; set; } = false;
        public UserType UserType { get; set; } = UserType.Internal;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
      
    }

}
