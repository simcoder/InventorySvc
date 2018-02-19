using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GOC.Inventory.Infrastructure.Configurations
{
    public class InventoryEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModels.InventoryAggregate.Inventory>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModels.InventoryAggregate.Inventory> builder)
        {
            builder.ToTable("Inventories");
            builder.Ignore(e=>e.Events);
            builder.HasKey(o => o.Id);
        }
    }
}
