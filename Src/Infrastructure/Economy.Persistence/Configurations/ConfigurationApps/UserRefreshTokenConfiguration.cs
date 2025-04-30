using Economy.Domain.Entites.EntityAppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationApps
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<AppUserRefreshToken>
	{
		public void Configure(EntityTypeBuilder<AppUserRefreshToken> builder)
        {
			builder.ToTable("UserRefreshTokens");
            builder.HasKey(x => x.UserId);
			builder.Property(x => x.Token).IsRequired();
		}
	}
}
