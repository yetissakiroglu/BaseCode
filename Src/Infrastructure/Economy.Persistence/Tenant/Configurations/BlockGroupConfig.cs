using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.Configurations
{
    public class BlockGroupConfig : IEntityTypeConfiguration<BlockGroup>
    {
        public void Configure(EntityTypeBuilder<BlockGroup> b)
        {
            b.ToTable("BlockGroups");
            b.HasKey(x => x.Id);

            //b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            //b.Property(x => x.Description).HasMaxLength(1000);

            b.HasMany(x => x.Items)
            .WithOne(i => i.BlockGroup)
            .HasForeignKey(i => i.BlockGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
