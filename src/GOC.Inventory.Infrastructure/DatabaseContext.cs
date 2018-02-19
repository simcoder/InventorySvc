using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using GOC.Inventory.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GOC.Inventory.Infrastructure
{
    public class DatabaseContext : DbContext, IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext(DbContextOptions options) : base (options)
        {
        }

        //required by EF
        public DatabaseContext(){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VendorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryEntityTypeConfiguration());
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Domain.AggregatesModels.InventoryAggregate.Inventory> Inventories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseNpgsql("Server=vagrant;Port=5432;Database=GOC_Inventory;Username=goc_postgres;Password=Robins01");

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
