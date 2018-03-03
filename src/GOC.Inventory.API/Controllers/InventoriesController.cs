using System;
using System.Threading.Tasks;
using GOC.Inventory.API.Application.DTOs;
using GOC.Inventory.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class InventoriesController : BaseController<InventoriesController>
    {
        readonly IInventoryService _inventoryService;
        public InventoriesController(IInventoryService inventoryService, ILoggerFactory loggerFactory) : base (loggerFactory)
        {
            _inventoryService = inventoryService;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InventoryDto inventory)
        {
            inventory.UserId = UserInfo.UserId;
            var result = await _inventoryService.CreateInventoryAsync(inventory);
            return Ok(result);
        }

        [HttpPut("/api/companies/{companyId}/[controller]/{inventoryId}")]
        public async Task<IActionResult> AddOrRemoveInventoryItem(Guid companyId, Guid inventoryId, [FromQuery]Guid itemId, [FromQuery]string command)
        {
            if(command == "add")
            {
                // add inventory
            }
            else if(command == "remove")
            {
                //remove item
            }
            return Ok();
        }

    }
}
