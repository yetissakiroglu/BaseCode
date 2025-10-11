using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Admin.Configurations.ConfigurationApps
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
	{
		public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {  
			builder.ToTable("UserClaims");
		}
	}
}
