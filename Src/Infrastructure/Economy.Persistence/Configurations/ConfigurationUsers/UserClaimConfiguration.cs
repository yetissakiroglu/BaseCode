using Economy.Domain.Entites.Identities;
using Economy.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
	{
		public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {  
			builder.ToTable("UserClaims");
		}
	}
}
// Tablo adlarını değiştir
//builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
