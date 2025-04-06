using Economy.Domain.Entites.EntityAppSettings;
using Economy.Domain.Entites.EntitySlides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppSlide
{
    public class AppSlide_Configuration : IEntityTypeConfiguration<AppSlide>
    {
        public void Configure(EntityTypeBuilder<AppSlide> builder)
        {
            builder.ToTable("AppSlides");

            builder.HasKey(x => x.Id);


            var models = new List<AppSlide>
                         {
                             new AppSlide{Sequence = 1,Thumbnail = "slide_1.jpg",AppSectionId=1,IsDeleted=false,Id=1},
                             new AppSlide{Sequence = 1,Thumbnail = "slide_2.jpg",AppSectionId=1,IsDeleted=false,Id=2},
                             new AppSlide{Sequence = 1,Thumbnail = "slide_3.jpg",AppSectionId=1,IsDeleted=false,Id=3}
                         };

            // Seed Data
            builder.HasData(models);
        }
    }


}
