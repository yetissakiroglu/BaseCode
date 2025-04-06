using Economy.Domain.Entites.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPage
{
    public class AppSection_Configuration : IEntityTypeConfiguration<AppSection>
    {
        public void Configure(EntityTypeBuilder<AppSection> builder)
        {
            builder.ToTable("AppSections");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.Content)
                   .HasMaxLength(maxLength:1000);


            var models = new List<AppSection>()
            {
                new AppSection
            {
                Id = 1,
                Name = "Anasayfa Slider",
                Content = "",
                SectionType = Domain.Enums.SectionType.Slide,
                IsDeleted = false
            },
                new AppSection
            {
                Id = 2,
                Name = "Imperial Turkiz Resort Hotel",
                Content = "Doğanın nefes kesen güzelliğinin turkuaz sularla buluştuğu Kemer’ in kalbinde konumlanan Türkiz Resort Hotel göz alıcı mimarisi ve sıcak atmosferi ile sizi eşsiz bir mutluluğa davet ediyor.",
                SectionType = Domain.Enums.SectionType.Corporate,
                IsDeleted = false
            }
            };

            // Seed Data
            builder.HasData(models);
        }
    }

   
}
