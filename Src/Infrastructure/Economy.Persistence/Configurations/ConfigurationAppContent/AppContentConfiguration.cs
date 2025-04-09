using Economy.Domain.Entites.EntityAppContents.AppContents;
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

            builder.Property(x => x.Thumbnail)
                .HasMaxLength(500); // Opsiyonel ama uzunluk kısıtı iyi olur

            builder.Property(x => x.ContentType)
                .IsRequired();

            builder.Property(x => x.PublicationStatus)
                .IsRequired();

            builder.HasOne(x => x.AppCategory)
                .WithMany()
                .HasForeignKey(x => x.AppCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategori silinirse içerik silinmesin

            builder.HasMany(x => x.Translations)
                .WithOne()
                .HasForeignKey(x => x.AppContentId) // Shadow property kullanılabilir veya navigation eklenebilir
                .OnDelete(DeleteBehavior.Cascade); // İçerik silinirse çeviriler de silinsin
        }
    }
}
