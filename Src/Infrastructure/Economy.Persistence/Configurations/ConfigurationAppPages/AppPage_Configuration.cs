using Economy.Domain.Entites.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPages
{
    public class AppPage_Configuration : IEntityTypeConfiguration<AppPage>
    {
        public void Configure(EntityTypeBuilder<AppPage> builder)
        {

            // Tablo adı (isteğe bağlı)
            builder.ToTable("AppPages");

            // Anahtar
            builder.HasKey(p => p.Id);

            //// Özellikler
            //builder.Property(p => p.Title)
            //       .IsRequired()
            //       .HasMaxLength(200);

            //builder.Property(p => p.Description)
            //       .HasMaxLength(1000);

            //builder.Property(p => p.Url)
            //       .IsRequired()
            //       .HasMaxLength(500); // URL için uygun bir maksimum uzunluk belirleyin

            builder.Property(p => p.IsHomePage)
                   .IsRequired();

            // İlişkiler
            builder.HasMany(p => p.AppPageSections)
                   .WithOne(ps => ps.AppPage)
                   .HasForeignKey(ps => ps.AppPageId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
  
}
