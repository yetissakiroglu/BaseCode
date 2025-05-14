using Economy.Domain.Entites.EntityAppLanguage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppLanguage
{
    public class AppLanguage_Configuration : IEntityTypeConfiguration<AppLanguage>
    {
        public void Configure(EntityTypeBuilder<AppLanguage> entity)
        {
            // 🔑 Primary Key
            entity.HasKey(x => x.Id);

            // 📌 Property Configurations (Opsiyonel - istersen burada property'leri özelleştirebilirsin)
            entity.Property(x => x.Code).IsRequired().HasMaxLength(10);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Icon).HasMaxLength(50);

            var languages = new List<AppLanguage>
                           {
                               new AppLanguage
                               {
                                   Id         = 1,
                                   Code       = "tr",
                                   Name       = "Türkçe",
                                   IsRTL      = false,
                                   Icon       = "flag-icon flag-icon-tur",
                                   IsActive   = true,
                                   IsDefault  = true,
                                   IsDeleted  = false
                               },
                               new AppLanguage
                                {
                                    Id         = 2,
                                    Code       = "en",
                                    Name       = "English",
                                    IsRTL      = false,
                                    Icon       = "flag-icon flag-icon-gbr",
                                    IsActive   = true,
                                    IsDefault  = false,
                                    IsDeleted  = false
                                },
                                                           new AppLanguage
                                {
                                    Id         = 3,
                                    Code       = "ar",
                                    Name       = "العربية",
                                    IsRTL      = true,
                                    Icon       = "flag-icon flag-icon-sau",
                                    IsActive   = true,
                                    IsDefault  = false,
                                    IsDeleted  = false
                                }
                           };

            // 🚀 Seed Data
            entity.HasData(languages);
        }
    }


}
