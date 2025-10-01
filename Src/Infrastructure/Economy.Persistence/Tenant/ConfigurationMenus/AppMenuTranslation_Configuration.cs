using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.ConfigurationMenus
{
    public class AppMenuTranslation_Configuration : IEntityTypeConfiguration<AppMenuTranslation>
    {
        public void Configure(EntityTypeBuilder<AppMenuTranslation> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(x => x.Id);

            // 🌐 Relation: AppMenu (Many-to-One)
            builder
                .HasOne<AppMenu>()
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.AppMenuId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🌐 Relation: AppLanguage (Many-to-One)
            builder
                .HasOne(x => x.AppLanguage)
                .WithMany()
                .HasForeignKey(x => x.AppLanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧩 Property Configs
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(x => x.Url)
                   .IsRequired()
                   .HasMaxLength(500);

            // 🗂️ Table Name
            builder.ToTable("AppMenuTranslations");

            builder.HasData(
    new AppMenuTranslation { Id = 1, AppMenuId = 1, AppLanguageId = 1, Title = "Ana Menü", Url = "ana-menu" },
    new AppMenuTranslation { Id = 2, AppMenuId = 1, AppLanguageId = 2, Title = "Main Menu", Url = "main-menu" },

    new AppMenuTranslation { Id = 3, AppMenuId = 2, AppLanguageId = 1, Title = "Odalar & Süitler", Url = "odalar-suitler" },
    new AppMenuTranslation { Id = 4, AppMenuId = 2, AppLanguageId = 2, Title = "Rooms & Suites", Url = "rooms-suites" },

    new AppMenuTranslation { Id = 5, AppMenuId = 3, AppLanguageId = 1, Title = "Restoran & Bar", Url = "restoran-bar" },
    new AppMenuTranslation { Id = 6, AppMenuId = 3, AppLanguageId = 2, Title = "Restaurant & Bar", Url = "restaurant-bar" },

    new AppMenuTranslation { Id = 7, AppMenuId = 4, AppLanguageId = 1, Title = "Spa & Wellness", Url = "spa-wellness" },
    new AppMenuTranslation { Id = 8, AppMenuId = 4, AppLanguageId = 2, Title = "Spa & Wellness", Url = "spa-wellness" },

    new AppMenuTranslation { Id = 9, AppMenuId = 5, AppLanguageId = 1, Title = "Hakkımızda", Url = "hakkimizda" },
    new AppMenuTranslation { Id = 10, AppMenuId = 5, AppLanguageId = 2, Title = "About Us", Url = "about-us" },

    new AppMenuTranslation { Id = 11, AppMenuId = 6, AppLanguageId = 1, Title = "İletişim", Url = "iletisim" },
    new AppMenuTranslation { Id = 12, AppMenuId = 6, AppLanguageId = 2, Title = "Contact", Url = "contact" }
);
        }
    }
}
