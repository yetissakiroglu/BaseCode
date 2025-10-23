using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.Configurations
{
    public class BlockGroupConfig : IEntityTypeConfiguration<AppBlockGroup>
    {
        public void Configure(EntityTypeBuilder<AppBlockGroup> b)
        {
            b.ToTable("BlockGroups");
            b.HasKey(x => x.Id);
 
        }
    }
}
