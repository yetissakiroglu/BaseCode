using Economy.Domain.Entites.EntityCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Economy.Persistence.Configurations.ConfigurationAppContents
{
    public class AppCategory_Configuration : IEntityTypeConfiguration<AppCategory>
    {
        public void Configure(EntityTypeBuilder<AppCategory> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Property configurations
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.MetaTitle)
                .HasMaxLength(150);

            builder.Property(x => x.MetaDescription)
                .HasMaxLength(300);

          

            // AppCategory için kendine referans veren ilişki yapılandırması
            builder
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Sonsuz döngüyü önlemek için



        }
    }
}
