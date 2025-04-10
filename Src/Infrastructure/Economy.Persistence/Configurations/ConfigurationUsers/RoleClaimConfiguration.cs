using Economy.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
	{
		public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {  
			builder.ToTable("RoleClaims");
		}
	}
}
