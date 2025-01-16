using Economy.Domain.Entites.EntityMenuItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations
{
    public class AppMenuConfiguration : IEntityTypeConfiguration<AppMenu>
    {
        public void Configure(EntityTypeBuilder<AppMenu> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(m => m.Slug)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(m => m.ParentMenu)
                .WithMany(m => m.SubMenus)
                .HasForeignKey(m => m.ParentMenuId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid circular cascade deletes
        }
    }
}
