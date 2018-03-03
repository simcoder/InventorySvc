using System;
using System.Threading.Tasks;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class ItemRepository : RepositoryBase<ItemRepository>, IItemRepository
    {
        private  readonly DatabaseContext _context;
        public ItemRepository(DatabaseContext context, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _context = context;
        }

        public async Task<Item> CreateInventoryItem(Item inventoryItem)
        {
            bool vendorExists = await _context.Vendors.FindAsync(inventoryItem.VendorId) != null;

            if(!vendorExists)
            {
                throw new Exception($"vendor id {inventoryItem.VendorId} does not exist");
            }

            bool companyExists = await _context.Companies.FindAsync(inventoryItem.CompanyId) != null;

            if (!companyExists)
            {
                throw new Exception($"company id {inventoryItem.CompanyId} does not exist");
            }
            try
            {
                await _context.Items.AddAsync(inventoryItem);
                await _context.SaveChangesAsync();
                //raise event
            }
            catch(Exception ex)
            {
                Logger.LogError(ex,"Error while saving item");
                throw ex;
            }

            return inventoryItem;
        }

        public async Task DeleteInventoryItem(Guid id, Guid userId)
        {
            try
            {
                Item item = await _context.Items.FindAsync(id);
                item.DeleteItem(userId);
                await _context.SaveChangesAsync();
                //raise event
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while deleting item");
                throw ex;
            }
        }

        public async Task MarkAsSold(Guid id, Guid companyId, Guid userId)
        {
            try
            {
                Item item = await _context.Items.FindAsync(id);

                if(item == null)
                {
                    throw new Exception($"item id {id} does not exist");
                }
                bool companyExists = await _context.Companies.FindAsync(companyId) != null;
                if (!companyExists)
                {
                    throw new Exception($"company id {companyId} does not exist");
                }
                item.SaleItem(companyId);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, "Error while marking item as sold");
                throw ex;
            }
        }
    }
}
