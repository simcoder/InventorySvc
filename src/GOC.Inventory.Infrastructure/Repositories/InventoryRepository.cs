using System;
using System.Linq;
using System.Threading.Tasks;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using GOC.Inventory.Domain.Events;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class InventoryRepository : RepositoryBase<InventoryRepository>, IInventoryRepository
    {
        private readonly DatabaseContext _context;

        public InventoryRepository(DatabaseContext context,ILoggerFactory loggerFactory) : base (loggerFactory)
        {
            _context = context;
        }

        public async Task AddInventoryItemAsync(Guid itemId, Guid inventoryId)
        {
            try
            {
                var item = await _context.Items.FindAsync(itemId);
                if(item == null)
                {
                    throw new Exception($"item id {item.Id} does not exist");
                }
                var inventory = await _context.Inventories.FindAsync(inventoryId);
                if (inventory == null)
                {
                    throw new Exception($"inventory id {inventoryId} does not exist");
                }
                inventory.AddInventoryItem(item);
                await _context.SaveChangesAsync();
                var addInventoryItem = (InventoryItemAdded)inventory.Events.Single(x => x.GetType() == typeof(InventoryItemAdded));
                DomainEvents.RaiseAsync(addInventoryItem);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, "Error while adding item to inventory");
                throw ex;
            }
        }

        public async Task<Domain.AggregatesModels.InventoryAggregate.Inventory> CreateInventoryAsync(Domain.AggregatesModels.InventoryAggregate.Inventory inventory)
        {
            try
            {
                bool companyExist = await _context.Companies.FindAsync(inventory.CompanyId) != null;

                if(!companyExist)
                {
                    throw new Exception($"company id {inventory.CompanyId} does not exist");
                }
                await _context.Inventories.AddAsync(inventory);
                await _context.SaveChangesAsync();
                var createdInventoryEvent = (InventoryCreated) inventory.Events.Single(x => x.GetType() == typeof(InventoryCreated));
                DomainEvents.RaiseAsync(createdInventoryEvent);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex,"Error while saving inventory");
                throw ex;
            }

            return inventory;
        }

        public async Task RemoveInventoryItemAsync(Guid itemId, Guid inventoryId)
        {
            try
            {
                var item = await _context.Items.FindAsync(itemId);
                if (item == null)
                {
                    throw new Exception($"item id {item.Id} does not exist");
                }
                var inventory = await _context.Inventories.FindAsync(inventoryId);
                if (inventory == null)
                {
                    throw new Exception($"inventory id {inventoryId} does not exist");
                }
                inventory.RemoveInventoryItem(item);
                await _context.SaveChangesAsync();
                var removedInventoryItem = (InventoryItemRemoved)inventory.Events.Single(x => x.GetType() == typeof(InventoryItemRemoved));
                DomainEvents.RaiseAsync(removedInventoryItem);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while removing item from inventory");
                throw ex;
            }
        }
    }
}
