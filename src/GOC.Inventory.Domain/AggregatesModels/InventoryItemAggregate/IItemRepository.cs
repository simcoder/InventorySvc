using System;
using System.Threading.Tasks;

namespace GOC.Inventory.Domain.AggregatesModels.InventoryAggregate
{
    public interface IItemRepository 
    {
        Task<Item> CreateInventoryItem(Item inventoryItem);

        Task MarkAsSold(Guid id, Guid companyId, Guid userId);

        Task DeleteInventoryItem(Guid id, Guid userId);
    }
}
