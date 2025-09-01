using Economy.Domain.Entites.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security;

namespace Economy.Base.Persistence.Configurations.ConfigurationApps
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

               var adminUser = new List<AppUser> {
                   
               new AppUser
               {
                   Id = 1,
                   JobTitle = "Süper Admin",
                   UserName = "Admin",
                   NormalizedUserName = "ADMIN",
                   Email = "yetissakiroglu@gmail.com",
                   NormalizedEmail = "YETISSAKIROGLU@GMAIL.COM",
                   EmailConfirmed = true,
                   FirstName = "Yetiş",
                   LastName = "Şakiroğlu",
                   IsDefaultAdmin = true,
                   SecurityStamp = "11111111-aaaa-bbbb-cccc-222222222222",
                   ConcurrencyStamp = "33333333-dddd-eeee-ffff-444444444444",
                   PasswordHash = "AQAAAAIAAYagAAAAENTd6wlppRLil0VbnPSjSF66HtD4Ckjs1Uraqpgi3/41X9LTDtE+ANyVCJQLfpjVyw==",
               }
            };

            builder.HasData(adminUser);

        }
    }
}
