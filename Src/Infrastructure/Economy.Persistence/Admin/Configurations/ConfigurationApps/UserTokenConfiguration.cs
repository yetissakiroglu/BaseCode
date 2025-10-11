using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Admin.Configurations.ConfigurationApps
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
	{
		public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {  
			builder.ToTable("UserTokens");
		}
	}
}
