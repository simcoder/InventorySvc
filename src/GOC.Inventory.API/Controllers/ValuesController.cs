using System.Threading.Tasks;
using GOC.Inventory.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class ValuesController : BaseController<ValuesController>
    {
        readonly IInventoryService _service;

        public ValuesController(ILoggerFactory loggerFactory, IInventoryService service) : base (loggerFactory)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.CreateInventoryAsync(new Application.DTOs.InventoryDto { });

            return Ok("success");

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
