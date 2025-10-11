using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.Configurations
{
    public class BlockItemImageConfig : IEntityTypeConfiguration<BlockItemImage>
    {
        public void Configure(EntityTypeBuilder<BlockItemImage> b)
        {
            b.ToTable("BlockItemImages");
            b.HasKey(x => x.Id);


            b.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired();
            b.Property(x => x.AltText).HasMaxLength(200);
        }
    }
}
