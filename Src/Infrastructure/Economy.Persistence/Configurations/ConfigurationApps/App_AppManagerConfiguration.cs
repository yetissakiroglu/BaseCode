using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationApps
{
    public class App_AppManagerConfiguration : IEntityTypeConfiguration<AppManager>
    {
        public void Configure(EntityTypeBuilder<AppManager> builder)
        {
            builder.ToTable("AppManagers");
            builder.HasKey(x => x.Id);

            builder.HasData();

        }
    }


}
