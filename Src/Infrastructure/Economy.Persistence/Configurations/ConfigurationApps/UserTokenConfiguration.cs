using Economy.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationApps
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
	{
		public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {  
			builder.ToTable("UserTokens");
		}
	}
}
