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
                   UserName = "Hotel1",
                   NormalizedUserName = "Hotel1",
                   Email = "Hotel1@example.com",
                   NormalizedEmail = "HOTEL1@EXAMPLE.COM",
                   EmailConfirmed = true,
                   FirstName = "Hotel1",
                   LastName = "Yöneticisi",
                   IsDefaultAdmin = true,
                   SecurityStamp = "11111111-aaaa-bbbb-cccc-222222222222",
                   ConcurrencyStamp = "33333333-dddd-eeee-ffff-444444444444",
                   PasswordHash = "AQAAAAIAAYagAAAAENTd6wlppRLil0VbnPSjSF66HtD4Ckjs1Uraqpgi3/41X9LTDtE+ANyVCJQLfpjVyw==",
                   TenantId = 1,
                   //Bu örnek hash değeri "Admin123*" parolası için geçerlidir.
               },
               new AppUser {
                    Id = 2,
                    UserName = "Hotel2",
                    NormalizedUserName = "Hotel2",
                    Email = "Hotel1@example.com",
                    NormalizedEmail = "HOTEL2@EXAMPLE.COM",
                    EmailConfirmed = true,
                    FirstName = "Hotel2",
                    LastName = "Yöneticisi",
                        IsDefaultAdmin = true,
                    SecurityStamp = "11111111-aaaa-bbbb-cccc-222222222222",
                        ConcurrencyStamp = "33333333-dddd-eeee-ffff-444444444444",
                    PasswordHash = "AQAAAAIAAYagAAAAEGhEU2J20Dt9rbBXKRMbF5MaTTD8UzKKRrYn+gZZfQsOImpHd+x/0sY1AA++BQV4Xw==",
                    TenantId = 2,
                    //Bu örnek hash değeri "Admin123*" parolası için geçerlidir.
                }
            };

            builder.HasData(adminUser);

        }
    }
}
