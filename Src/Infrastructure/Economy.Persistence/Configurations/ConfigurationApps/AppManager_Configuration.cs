using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Persistence.Configurations.ConfigurationApps
{
    public class AppManager_Configuration : IEntityTypeConfiguration<AppManager>
    {
        public void Configure(EntityTypeBuilder<AppManager> builder)
        {
            builder.ToTable("AppManagers");
            builder.HasKey(x => x.Id);

            builder.HasData();

        }
    }


}
