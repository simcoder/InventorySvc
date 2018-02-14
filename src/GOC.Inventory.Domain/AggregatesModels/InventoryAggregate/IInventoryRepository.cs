using System;
using System.Threading.Tasks;

namespace GOC.Inventory.Domain.AggregatesModels.InventoryAggregate
{
    public interface IInventoryRepository
    {
        Task<Inventory> CreateInventoryAsync (Inventory inventory);

        Task AddInventoryItemAsync(Item item);

        Task RemoveInventoryItemAsync(Guid ItemId);
    }
}
