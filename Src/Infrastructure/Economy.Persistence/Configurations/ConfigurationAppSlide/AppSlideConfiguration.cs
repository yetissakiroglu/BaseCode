using Economy.Domain.Entites.EntitySlides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppSlide
{
    

    public class AppSlideConfiguration : IEntityTypeConfiguration<AppSlide>
    {
        public void Configure(EntityTypeBuilder<AppSlide> builder)
        {
            builder.ToTable("AppSlides");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Sequence).IsRequired();

            builder.Property(x => x.ThumbnailBase64)
                   .HasColumnType("nvarchar(max)");

            builder.Property(x => x.ThumbnailMobilBase64)
                   .HasColumnType("nvarchar(max)");

            builder.HasMany(x => x.Translations)
                   .WithOne()
                   .HasForeignKey(x => x.AppSlideId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 3 adet slide seed verisi
            builder.HasData(
                new AppSlide
                {
                    Id = 1,
                    Sequence = 1,
                    ThumbnailBase64 = "data:image/png;base64,AAA_SLIDE1",
                    ThumbnailMobilBase64 = "data:image/png;base64,AAA_SLIDE1_MOBILE"
                },
                new AppSlide
                {
                    Id = 2,
                    Sequence = 2,
                    ThumbnailBase64 = "data:image/png;base64,BBB_SLIDE2",
                    ThumbnailMobilBase64 = "data:image/png;base64,BBB_SLIDE2_MOBILE"
                },
                new AppSlide
                {
                    Id = 3,
                    Sequence = 3,
                    ThumbnailBase64 = "data:image/png;base64,CCC_SLIDE3",
                    ThumbnailMobilBase64 = "data:image/png;base64,CCC_SLIDE3_MOBILE"
                }
            );
        }
    }



}
