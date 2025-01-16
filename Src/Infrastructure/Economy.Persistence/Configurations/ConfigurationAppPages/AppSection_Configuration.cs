using Economy.Domain.Entites.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPages
{
    public class AppSection_Configuration : IEntityTypeConfiguration<AppSection>
    {
        public void Configure(EntityTypeBuilder<AppSection> builder)
        {

            // Tablo adı (isteğe bağlı)
            builder.ToTable("AppSections");

            // Anahtar
            builder.HasKey(s => s.Id);


            builder.Property(s => s.Content)
                   .HasColumnType("nvarchar(max)"); // İçerik tipi (isteğe bağlı)

            builder.Property(s => s.SectionType)
                   .IsRequired();

            // İlişkiler
            builder.HasMany(s => s.AppPageSections)
                   .WithOne(ps => ps.AppSection)
                   .HasForeignKey(ps => ps.AppSectionId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

