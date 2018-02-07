using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        readonly ILogger _logger;

        public ValuesController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ValuesController>();
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = new string[] { "value1", "value2" };

            return Ok(result);

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
