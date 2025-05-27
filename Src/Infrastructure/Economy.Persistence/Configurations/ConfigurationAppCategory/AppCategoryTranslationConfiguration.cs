using Economy.Domain.Entites.EntityAppCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Economy.Base.Persistence.Configurations.ConfigurationAppCategory
{
    public class AppCategoryTranslationConfiguration : IEntityTypeConfiguration<AppCategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<AppCategoryTranslation> builder)
        {
            builder.ToTable("AppCategoryTranslations");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(t => t.Url)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(t => t.AppLanguageId)
                   .IsRequired();

            builder.HasIndex(t => new { t.AppCategoryId, t.AppLanguageId })
                   .IsUnique(); // Her dil için bir çeviri

            builder.HasOne(t => t.AppCategory)
                   .WithMany(c => c.Translations)
                   .HasForeignKey(t => t.AppCategoryId);


            builder.HasData(
    new AppCategoryTranslation
    {
        Id = 1,
        AppCategoryId = 1,
        AppLanguageId = 1, // Örneğin Türkçe
        Title = "Genel",
        Url = "genel",
        ShortDescription = "Genel açıklama",
        Content = "Genel içerik"
    },
    new AppCategoryTranslation
    {
        Id = 2,
        AppCategoryId = 2,
        AppLanguageId = 1,
        Title = "Alt Kategori",
        Url = "alt-kategori",
        ShortDescription = "Alt açıklama",
        Content = "Alt içerik"
    }
);


        }
    }
}
