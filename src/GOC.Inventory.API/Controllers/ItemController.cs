using GOC.Inventory.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ItemController : BaseController<InventoriesController>
    {
        readonly IItemService _itemService;
        public ItemController(IItemService itemService, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _itemService = itemService;
        }
    }
}
