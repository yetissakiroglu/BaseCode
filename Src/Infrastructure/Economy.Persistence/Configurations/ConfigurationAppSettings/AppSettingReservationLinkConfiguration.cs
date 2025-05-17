using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationAppSettings
{
    public class AppSettingReservationLinkConfiguration : IEntityTypeConfiguration<AppSettingReservationLink>
    {
        public void Configure(EntityTypeBuilder<AppSettingReservationLink> builder)
        {
            builder.ToTable("AppSettingReservationLinks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasData(
                new AppSettingReservationLink
                {
                    Id = 1,
                    Url = "https://example.com/reservation",
                    IsDeleted = false
                }
            );
        }
    }
}
