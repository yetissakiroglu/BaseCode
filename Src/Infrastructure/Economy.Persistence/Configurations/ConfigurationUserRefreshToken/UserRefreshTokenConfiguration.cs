using Economy.Domain.Entites.EntityAppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
	public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
	{
		public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
		{
			builder.HasKey(x => x.UserId);
			builder.Property(x => x.Token).IsRequired();
		}
	}
}
