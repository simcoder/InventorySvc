using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GOC.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class InventoriesController
    {
        public InventoriesController()
        {
        }
    }
}
