using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.ConfigurationMenus
{
    public class AppMenu_Configuration : IEntityTypeConfiguration<AppMenu>
    {
        public void Configure(EntityTypeBuilder<AppMenu> builder)
        {

            // 🔑 Primary Key
            builder.HasKey(x => x.Id);

            // 🧭 Self-referencing: ParentMenu - SubMenus
            builder.HasOne(x => x.Parent)
         .WithMany(x => x.Children)
         .HasForeignKey(x => x.ParentId)
         .OnDelete(DeleteBehavior.Restrict);

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


        }
    }
}
