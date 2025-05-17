using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationAppSettings
{
    public class AppSettingReservationNumberConfiguration : IEntityTypeConfiguration<AppSettingReservationNumber>
    {
        public void Configure(EntityTypeBuilder<AppSettingReservationNumber> builder)
        {
            builder.ToTable("AppSettingReservationNumbers");

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
                new AppSettingReservationNumber
                {
                    Id = 1,
                    CountryCode = "+90",
                    Number = "5551112233",
                    IsPrimary = true,
                    IsDeleted = false
                },
                new AppSettingReservationNumber
                {
                    Id = 2,
                    CountryCode = "+1",
                    Number = "2025550123",
                    IsPrimary = false,
                    IsDeleted = false
                }
            );
        }
    }
}
