using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GOC.Inventory.API.Application.DTOs;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;

namespace GOC.Inventory.API.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync();
        Task<InventoryDto> CreateInventoryAsync(InventoryDto inventory);
        Task<bool> AddItemToInventoryAsync(Item item);
        Task<bool> RemoveItemFromInventoryAsync(Guid itemId);
    }
}
