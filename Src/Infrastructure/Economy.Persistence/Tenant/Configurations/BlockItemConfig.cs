using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.Configurations
{
    public class BlockItemConfig : IEntityTypeConfiguration<BlockItem>
    {
        public void Configure(EntityTypeBuilder<BlockItem> b)
        {
            b.ToTable("BlockItems");
            b.HasKey(x => x.Id);


            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Summary).HasMaxLength(1000);
            b.Property(x => x.CoverImage).HasMaxLength(500).IsRequired();


            b.Property(x => x.ExternalUrl).HasMaxLength(500);
            b.Property(x => x.Target).HasMaxLength(20);
        }
    }
}
