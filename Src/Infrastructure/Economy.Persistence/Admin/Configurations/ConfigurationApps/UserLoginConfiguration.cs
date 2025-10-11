using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Admin.Configurations.ConfigurationApps
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
	{
		public void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {   
            builder.ToTable("UserLogins");
            //builder.HasNoKey(); // Bu bir view veya key'siz bir yapı

        }
    }
}
