using Economy.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
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
