using Economy.Domain.Entites.AdminEntity.EntityApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Admin.Configurations.ConfigurationApps
{
    public class App_Configuration : IEntityTypeConfiguration<App>
    {
        public void Configure(EntityTypeBuilder<App> builder)
        {
            builder.ToTable("Apps");
            builder.HasKey(x => x.Id);

            builder.HasData(
            new App
            {
                Id = 1,
                HotelName = "Grand Ocean Resort",
                ServerName = "MSI",
                DatabaseName = "HotelDb1",
                UserName = "admin",
                IsPassword = false,
                Password = "SecurePassword123",
                Domain = "grand-ocean.com"
            },
            new App
            {
                Id = 2,
                HotelName = "Luxe Retreat",
                ServerName = "MSI",
                DatabaseName = "HotelDb2",
                UserName = "user1",
                IsPassword = false,
                Password = "",
                Domain = "luxeretreat.com"
            }
        );

        }
    }


}
