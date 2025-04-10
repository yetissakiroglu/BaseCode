using Economy.Domain.Entites.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationUserRefreshToken
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

            var adminUser = new AppUser
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                FirstName = "Sistem",
                LastName = "Yöneticisi",
                DateOfBirth = new DateTime(1990, 1, 1),
                IsDefaultAdmin = true,
                SecurityStamp = "11111111-aaaa-bbbb-cccc-222222222222",
                ConcurrencyStamp = "33333333-dddd-eeee-ffff-444444444444",
                PasswordHash = "AQAAAAEAACcQAAAAEGCzY20L2G+TswPL8nVZ7gCm+3OaKjk9iN9abVdTOf5wjMPVnljfMRZsWYixI4LSQg=="
                //Bu örnek hash değeri "Admin123*" parolası için geçerlidir.

            };

            builder.HasData(adminUser);
        }
    }
}
