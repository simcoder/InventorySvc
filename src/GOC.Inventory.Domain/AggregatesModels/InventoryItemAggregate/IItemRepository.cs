using System;
namespace GOC.Inventory.Domain.AggregatesModels.InventoryAggregate
{
    public interface IItemRepository 
    {
        Item CreateInventoryItem(Item inventoryItem);

        Item UpdateInventoryItem(Guid id, Item inventoryUpdated);

        bool DeleteInventoryItem(Guid id);
    }
}
