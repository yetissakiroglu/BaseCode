using Economy.Domain.Entites.EntityAppPages;
using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPage
{
    public class AppSectionImage_Configuration : IEntityTypeConfiguration<AppSectionImage>
    {
        public void Configure(EntityTypeBuilder<AppSectionImage> builder)
        {
            builder.ToTable("AppSectionImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.Content)
                   .HasMaxLength(maxLength:1000);


            var models = new List<AppSectionImage>()
            {
                new AppSectionImage
            {   AppSectionId = 2,
                Id = 1,
                Name = "Kurumsal 1",
                Sequence = 1,
                Content = "",
                IsDeleted = false,
                Thumbnail = "essiz-misafirperverligi-595bb.jpg"
            },
                new AppSectionImage
            {
                    AppSectionId = 2,
                Id = 2,
                Name = "Kurumsal 2",
                Sequence = 2,
                Content = "",
                IsDeleted = false,
                Thumbnail = "essiz-osmanli-stili-ve-misafirperverligi-7315a.jpg"
            }
            };

            // Seed Data
            builder.HasData(models);
        }
    }

   
}
