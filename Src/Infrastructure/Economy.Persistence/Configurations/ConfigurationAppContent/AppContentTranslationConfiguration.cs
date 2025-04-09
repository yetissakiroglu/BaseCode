using Economy.Domain.Entites.EntityAppContents.AppContents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppContent
{
    public class AppContentTranslationConfiguration : IEntityTypeConfiguration<AppContentTranslation>
    {
        public void Configure(EntityTypeBuilder<AppContentTranslation> builder)
        {
            builder.ToTable("AppContentTranslations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(500);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.MetaTitle)
                .HasMaxLength(150);

            builder.Property(x => x.MetaDescription)
                .HasMaxLength(300);

            builder.Property(x => x.Content)
                .HasColumnType("nvarchar(max)"); // Büyük içerikler için

            builder.Property(x => x.IsExternal)
                .IsRequired();

            builder.HasOne(x => x.AppLanguage)
                .WithMany()
                .HasForeignKey(x => x.AppLanguageId)
                .OnDelete(DeleteBehavior.Restrict); // Dile ait içerikler silinmesin
        }
    }
}
