using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GOC.Inventory.Infrastructure.Configurations
{
    public class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");
            builder.HasKey(o => o.Id);
            builder.Ignore(e => e.Events);

            //mobile phone item value object persisted as owned entity in EF Core 2.0
            builder.OwnsOne(o => o.MobilePhone);
        }
    }
}
