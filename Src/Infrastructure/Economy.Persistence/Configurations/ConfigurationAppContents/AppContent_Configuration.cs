using Economy.Domain.Entites.EntityPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppContents
{
    public class AppContent_Configuration : IEntityTypeConfiguration<AppContent>
    {
        public void Configure(EntityTypeBuilder<AppContent> builder)
        {
            // AppContent ile AppCategory arasındaki ilişki
            builder
                .HasOne(ac => ac.AppCategory)
                .WithMany(c => c.AppContents)
                .HasForeignKey(ac => ac.AppCategoryId);

        }
    }
}
