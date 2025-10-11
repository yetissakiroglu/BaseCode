using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Admin.Configurations.ConfigurationApps
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
	{
		public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {  
			builder.ToTable("RoleClaims");
		}
	}
}
