using System;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public ItemRepository()
        {
        }

        public Item CreateInventoryItem(Item inventoryItem)
        {
            throw new NotImplementedException();
        }

        public bool DeleteInventoryItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item UpdateInventoryItem(Guid id, Item inventoryUpdated)
        {
            throw new NotImplementedException();
        }
    }
}
