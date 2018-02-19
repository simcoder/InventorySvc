using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GOC.Inventory.Infrastructure.Configurations
{
    public class VendorEntityTypeConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("Vendors");
            builder.HasKey(o => o.Id);
            builder.Ignore(e => e.Events);
            // peristing value object
            builder.OwnsOne(o => o.Address);
        }
    }
}
