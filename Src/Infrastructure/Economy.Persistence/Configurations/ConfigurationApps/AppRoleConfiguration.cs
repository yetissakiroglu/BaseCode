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
                     Id = 1,
                     Name = "Admin",
                     NormalizedName = "ADMIN",
                     ConcurrencyStamp = "a1111111-b222-c333-d444-e55555555555"
                 },
                 new AppRole
                 {
                     Id = 2,
                     Name = "Otel Editör",
                     NormalizedName = "OTEL EDİTÖR", // veya "OTEL EDITOR" İngilizceye normalize edilmiş hali
                     ConcurrencyStamp = "f6666666-g777-h888-i999-j00000000000"
                 }
             });
        }
    }

}
