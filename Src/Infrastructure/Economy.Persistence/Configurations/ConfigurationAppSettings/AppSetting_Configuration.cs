using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppSettings
{
    public class AppSetting_Configuration : IEntityTypeConfiguration<AppSetting>
    {

        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {


        }
    }
}
