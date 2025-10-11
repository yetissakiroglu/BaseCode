using Economy.Domain.Entites.AdminEntity.EntityApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Admin.Configurations.ConfigurationApps
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
