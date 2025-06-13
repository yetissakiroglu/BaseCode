using Economy.Domain.Entites.EntityAppContents.AppContents;
using Economy.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppContent
{
    
    public class AppContentConfiguration : IEntityTypeConfiguration<AppContent>
    {
        public void Configure(EntityTypeBuilder<AppContent> builder)
        {
            builder.ToTable("AppContents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WebThumbnailUrl).HasMaxLength(500);
            builder.Property(x => x.MobilThumbnailUrl).HasMaxLength(500);

            builder.HasOne(x => x.AppCategory)
                   .WithMany()
                   .HasForeignKey(x => x.AppCategoryId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

            // Seed data
            builder.HasData(
                new AppContent
                {
                    Id = 1,
                    WebThumbnailUrl = "/images/content1-web.jpg",
                    MobilThumbnailUrl = "/images/content1-mobile.jpg",
                    ContentType = ContentType.Odalar,
                    AppCategoryId = null
                },
                new AppContent
                {
                    Id = 2,
                    WebThumbnailUrl = "/images/content2-web.jpg",
                    MobilThumbnailUrl = "/images/content2-mobile.jpg",
                    ContentType = ContentType.Hakkimizda,
                    AppCategoryId = 1
                }
            );
        }
    }
}
