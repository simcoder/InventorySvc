using System;
using System.Linq;
using System.Threading.Tasks;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using GOC.Inventory.Domain.Events;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DatabaseContext _context;

        public InventoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddInventoryItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.AggregatesModels.InventoryAggregate.Inventory> CreateInventoryAsync(Domain.AggregatesModels.InventoryAggregate.Inventory inventory)
        {
            try
            {
                await _context.AddAsync(inventory);
                await _context.SaveChangesAsync();
                var createdInventoryEvent = (InventoryCreated) inventory.Events.Single(x => x.GetType() == typeof(InventoryCreated));
                DomainEvents.RaiseAsync(createdInventoryEvent);
            }
            catch(Exception ex)
            {
                //log exception
                //raise error occurred event
            }

            return inventory;
        }

        public async Task RemoveInventoryItemAsync(Guid ItemId)
        {
            throw new NotImplementedException();
        }
    }
}
