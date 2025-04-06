using Economy.Domain.Entites.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPage
{
    public class AppPageSection_Configuration : IEntityTypeConfiguration<AppPageSection>
    {
        public void Configure(EntityTypeBuilder<AppPageSection> builder)
        {
            builder.ToTable("AppPageSections");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Content).HasMaxLength(1000);

            builder.HasOne(x => x.AppPage)
            .WithMany(p => p.AppPageSections)
            .HasForeignKey(x => x.AppPageId);

            builder.HasOne(x => x.AppSection)
                   .WithMany(s => s.AppPageSections)
                   .HasForeignKey(x => x.AppSectionId);

            //builder.HasData(new AppPageSection
            //{
            //    Id = 1,
            //    AppPageId = 1,
            //    AppSectionId = 1,
            //    Name = "Section 1",
            //    Content = "Content 1",
            //    Sequence = 1,
            //    IsVisible = true,
            //    IsDeleted = false
            //});
        }
    }
  
}
