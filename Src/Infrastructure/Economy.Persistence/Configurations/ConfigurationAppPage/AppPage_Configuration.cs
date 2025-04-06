using Economy.Domain.Entites.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPage
{
    public class AppPage_Configuration : IEntityTypeConfiguration<AppPage>
    {
        public void Configure(EntityTypeBuilder<AppPage> builder)
        {
            builder.ToTable("AppPages");

            builder.HasKey(x => x.Id);
            // Seed Data
            builder.HasData(new AppPage
            {
                Id = 1,
                IsHomePage = true,
                IsDeleted = false
            });
        }
    }
  
}
