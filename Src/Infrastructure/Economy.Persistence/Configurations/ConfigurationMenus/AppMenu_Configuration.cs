using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityMenuItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationMenuItems
{
    public class AppMenu_Configuration : IEntityTypeConfiguration<AppMenu>
    {
        public void Configure(EntityTypeBuilder<AppMenu> builder)
        {

            // 🔑 Primary Key
            builder.HasKey(x => x.Id);

            // 🧭 Self-referencing: ParentMenu - SubMenus
            builder
                .HasOne(x => x.ParentMenu)
                .WithMany(x => x.SubMenus)
                .HasForeignKey(x => x.ParentMenuId)
                .OnDelete(DeleteBehavior.Restrict); // Sonsuz döngüleri önlemek için

            // 🌐 Translations: One-to-Many
            builder
                .HasMany(x => x.Translations)
                .WithOne()
                .HasForeignKey(x => x.AppMenuId)
                .OnDelete(DeleteBehavior.Cascade);

            // ⚙️ IsExternal alanı
            builder
                .Property(x => x.IsExternal)
                .IsRequired();

            // 📦 Tablo Adı (isteğe bağlı)
            builder.ToTable("AppMenus");

            builder.HasData(
                new AppMenu { Id = 1, IsExternal = false, ParentMenuId = null },
                new AppMenu { Id = 2, IsExternal = false, ParentMenuId = null },
                new AppMenu { Id = 3, IsExternal = false, ParentMenuId = null },
                new AppMenu { Id = 4, IsExternal = false, ParentMenuId = null },
                new AppMenu { Id = 5, IsExternal = false, ParentMenuId = null },
                new AppMenu { Id = 6, IsExternal = false, ParentMenuId = null }
            );


        }
    }
}
