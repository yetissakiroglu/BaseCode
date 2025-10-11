using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.Configurations.ConfigurationAppSettings
{

    public class AppSetting_Configuration : IEntityTypeConfiguration<AppSetting>
    {
        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            builder.ToTable("AppSettings");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Translations)
                   .WithOne()
                   .HasForeignKey(x => x.AppSettingId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Seed
            builder.HasData(
                new AppSetting { Id = 1 }  // Sadece ID, diğerleri çeviri tablosunda olacak
            );
        }
    }

   
}
