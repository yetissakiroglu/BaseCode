using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Persistence.Configurations.ConfigurationAppLogoSetting
{
    public class AppLogoSetting_Configuration : IEntityTypeConfiguration<AppLogoSetting>
    {
        public void Configure(EntityTypeBuilder<AppLogoSetting> builder)
        {
            builder.ToTable("AppLogoSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WebLogoPath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.MobileLogoPath)
                   .IsRequired()
                   .HasMaxLength(500);

            // Seed Data
            builder.HasData(new AppLogoSetting
            {
                Id = 1,
                WebLogoPath = "img/logo.png",
                MobileLogoPath = "img/logo_m.png"
            });
        }
    }
   
}
