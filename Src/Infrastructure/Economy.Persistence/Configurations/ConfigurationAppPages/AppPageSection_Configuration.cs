using Economy.Domain.Entites.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPages
{
    public class AppPageSection_Configuration : IEntityTypeConfiguration<AppPageSection>
    {
        public void Configure(EntityTypeBuilder<AppPageSection> builder)
        {

            // Tablo adı (isteğe bağlı)
            builder.ToTable("AppPageSections");

            // Anahtar
            builder.HasKey(ps => ps.Id);

            //// Özellikler
            //builder.Property(ps => ps.Name)
            //       .IsRequired()
            //       .HasMaxLength(100);

            //builder.Property(ps => ps.Content)
            //       .HasColumnType("nvarchar(max)"); // İçerik tipi (isteğe bağlı)

            //builder.Property(ps => ps.IsVisible)
            //       .IsRequired();

            // İlişkiler
            builder.HasOne(ps => ps.AppPage)
                   .WithMany(p => p.AppPageSections)
                   .HasForeignKey(ps => ps.AppPageId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ps => ps.AppSection)
                   .WithMany(s => s.AppPageSections)
                   .HasForeignKey(ps => ps.AppSectionId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
   
}
