using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationApps
{

    public class App_GeneralSettingsConfiguration : IEntityTypeConfiguration<AppGeneralSetting>
    {
        public void Configure(EntityTypeBuilder<AppGeneralSetting> builder)
        {
            // Tablo
            builder.ToTable("AppGeneralSettings");

            // PK
            builder.HasKey(x => x.Id);

            // Zorunlu / Uzunluklar
            builder.Property(x => x.SiteName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Domain)
                   .HasMaxLength(200);

            builder.Property(x => x.Theme)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("light"); // Kod tarafındaki default ile uyumlu (opsiyonel)

            builder.Property(x => x.LogoUrl)
                   .HasMaxLength(300);

            builder.Property(x => x.MetaTitleSuffix)
                   .HasMaxLength(120);

            builder.Property(x => x.DefaultMetaDescription)
                   .HasMaxLength(300);

        }
    }

}
