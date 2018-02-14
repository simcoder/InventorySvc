using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GOC.Inventory.Infrastructure.Configurations
{
    public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");
            builder.HasKey(o => o.Id);
            builder.Ignore(e => e.Events);

            // peristing value object
            builder.OwnsOne(o => o.Address);
        }
    }
}
