using Economy.Domain.Entites.EntityAppUsers;
using Economy.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
	{
		public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasNoKey(); // Bu bir view veya key'siz bir yapı

        }
    }
}
