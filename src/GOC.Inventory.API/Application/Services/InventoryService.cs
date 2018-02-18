using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GOC.Inventory.API.Application.DTOs;
using GOC.Inventory.API.Application.Interfaces;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;

namespace GOC.Inventory.API.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepo;
        private readonly ICompanyRepository _companyRespository;
        private readonly IItemRepository _itemRespository;



        public InventoryService(IInventoryRepository inventoryRepo, ICompanyRepository companyRespository, IItemRepository itemRepository)
        {
            _inventoryRepo = inventoryRepo;
            _companyRespository = companyRespository;
            _itemRespository = itemRepository;
        }

        public async Task<InventoryDto> CreateInventoryAsync(InventoryDto inventory)
        {
            inventory.Id = Guid.NewGuid();
            var model = new Domain.AggregatesModels.InventoryAggregate.Inventory(inventory.CompanyId, inventory.Id, inventory.UserId);
            await _inventoryRepo.CreateInventoryAsync(model);
            return inventory;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync()
        {
            //get inventories
            //iterate thu list and use company repo to get company name with id
            throw new NotImplementedException();
        }

        public async Task<bool> AddItemToInventoryAsync(Item item)
        {
            //call item repo to create item
            //call inventory repo to add to inventoru
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveItemFromInventoryAsync(Guid itemId)
        {
            //call item repo to get item by id
            //call inventory repo to remove the item
            throw new NotImplementedException();
        }
    }
}
