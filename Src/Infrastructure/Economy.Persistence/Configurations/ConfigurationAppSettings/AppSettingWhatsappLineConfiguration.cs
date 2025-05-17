using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Base.Persistence.Configurations.ConfigurationAppSettings
{
    public class AppSettingWhatsappLineConfiguration : IEntityTypeConfiguration<AppSettingWhatsappLine>
    {
        public void Configure(EntityTypeBuilder<AppSettingWhatsappLine> builder)
        {
            builder.ToTable("AppSettingWhatsappLines");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CountryCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasIndex(x => new { x.CountryCode, x.Number }).IsUnique();

            builder.HasData(
                new AppSettingWhatsappLine
                {
                    Id = 1,
                    CountryCode = "+90",
                    Number = "5559998877",
                    IsPrimary = true,
                    IsDeleted = false
                },
                new AppSettingWhatsappLine
                {
                    Id = 2,
                    CountryCode = "+49",
                    Number = "15233445566",
                    IsPrimary = false,
                    IsDeleted = false
                }
            );
        }
    }
}
