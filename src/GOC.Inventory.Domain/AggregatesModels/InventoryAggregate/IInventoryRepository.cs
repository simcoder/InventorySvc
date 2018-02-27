using System;
using System.Threading.Tasks;

namespace GOC.Inventory.Domain.AggregatesModels.InventoryAggregate
{
    public interface IInventoryRepository
    {
        Task<Inventory> CreateInventoryAsync (Inventory inventory);

        Task AddInventoryItemAsync(Guid itemId, Guid inventoryId, Guid userId);

        Task RemoveInventoryItemAsync(Guid itemId, Guid inventoryId, Guid userId);
    }
}
