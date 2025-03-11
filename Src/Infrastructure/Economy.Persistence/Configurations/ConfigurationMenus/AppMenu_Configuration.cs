using Economy.Domain.Entites.EntityMenuItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationMenuItems
{
    public class AppMenu_Configuration : IEntityTypeConfiguration<AppMenu>
    {
        public void Configure(EntityTypeBuilder<AppMenu> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.ParentMenu)
                 .WithMany(m => m.SubMenus)
                 .HasForeignKey(m => m.ParentMenuId)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
