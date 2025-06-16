using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationAppCategory
{
    public class AppCategoryConfiguration : IEntityTypeConfiguration<AppCategory>
    {
        public void Configure(EntityTypeBuilder<AppCategory> builder)
        {
            builder.ToTable("AppCategories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ContentType)
                   .IsRequired();

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict); // Dairesel ilişkileri engellemek için

            builder.HasMany(c => c.Translations)
                   .WithOne(t => t.AppCategory)
                   .HasForeignKey(t => t.AppCategoryId);


            builder.HasData(
    new AppCategory
    {
        Id = 1,
        ContentType = ContentType.General, // enum değeri
        ParentCategoryId = null
    },
    new AppCategory
    {
        Id = 2,
        ContentType = ContentType.General,
        ParentCategoryId = 1
    }
);
        }
    }
}
