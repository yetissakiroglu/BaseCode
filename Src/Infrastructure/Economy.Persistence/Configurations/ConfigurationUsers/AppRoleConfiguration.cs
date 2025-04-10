using Economy.Domain.Entites.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");

            builder.HasData(new AppRole
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "a1111111-b222-c333-d444-e55555555555"
            });
        }
    }

}
