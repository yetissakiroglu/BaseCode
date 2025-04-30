using Economy.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationApps
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
	{
		public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {  
			builder.ToTable("RoleClaims");
		}
	}
}
