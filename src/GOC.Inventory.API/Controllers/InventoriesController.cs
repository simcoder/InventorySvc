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
            var result = await _inventoryService.CreateInventoryAsync(inventory);
            return Ok(result);
        }
    }
}
