using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Admin.Configurations.ConfigurationApps
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
	{
		public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasData(new IdentityUserRole<int>
            {
                UserId = 1,
                RoleId = 4
            });
            //builder.HasNoKey(); // Bu bir view veya key'siz bir yapı

        }
    }
}
