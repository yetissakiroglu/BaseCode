using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationAppSettings
{
    public class AppSettingLogoConfiguration : IEntityTypeConfiguration<AppSettingLogo>
    {
        public void Configure(EntityTypeBuilder<AppSettingLogo> builder)
        {
            builder.ToTable("AppSettingLogos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.LogoPath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.MobileLogoPath)
                   .HasMaxLength(500);

            builder.Property(x => x.FaviconPath)
                   .HasMaxLength(500);

            // Seed data
            builder.HasData(
                new AppSettingLogo
                {
                    Id = 1,
                    LogoPath = "/images/logo/default-logo.png",
                    MobileLogoPath = "/images/logo/mobile-logo.png",
                    FaviconPath = "/images/logo/favicon.ico"
                }
            );
        }
    }
}
