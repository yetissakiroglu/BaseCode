using Economy.Domain.Entites.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationApps
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");

            builder.HasData(new List<AppRole>
             {
                 new AppRole
                 {
                     Id = 4,
                     Name = "Süper Admin",
                     NormalizedName = "SUPER ADMIN",
                     ConcurrencyStamp = "a1111111-b222-c333-d444-e55555555555"
                 },
                 new AppRole
                 {
                     Id = 5,
                     Name = "Tenant Admin",
                     NormalizedName = "TENANT ADMIN",
                     ConcurrencyStamp = "f6666666-g777-h888-i999-j00000000000"
                 }
             });
        }
    }

}
