using Economy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
	{
		public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasData(new IdentityUserRole<int>
            {
                UserId = 1,
                RoleId = 1
            });
            //builder.HasNoKey(); // Bu bir view veya key'siz bir yapı

        }
    }
}
