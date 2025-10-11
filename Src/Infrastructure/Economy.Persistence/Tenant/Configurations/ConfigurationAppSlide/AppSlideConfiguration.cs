using Economy.Domain.Entites.TenantEntity.EntityAppSlides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.Configurations.ConfigurationAppSlide
{
    

    public class AppSlideConfiguration : IEntityTypeConfiguration<AppSlide>
    {
        public void Configure(EntityTypeBuilder<AppSlide> builder)
        {
            builder.ToTable("AppSlides");

            builder.HasKey(x => x.Id);

    

            builder.HasMany(x => x.Translations)
                   .WithOne()
                   .HasForeignKey(x => x.AppSlideId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 3 adet slide seed verisi
            builder.HasData(
                new AppSlide
                {
                    Id = 1
                    },
                new AppSlide
                {
                    Id = 2
                          },
                new AppSlide
                {
                    Id = 3
                                }
            );
        }
    }



}
