using Economy.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
	{
		public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {  
			builder.ToTable("UserTokens");
		}
	}
}
