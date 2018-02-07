using System;
using System.Threading.Tasks;
using EasyNetQ.Topology;
using GOC.Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GOC.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        readonly ILogger _logger;
        readonly IEventConsumer _con;
        readonly IEventPublisher _pub;

        public ValuesController(ILoggerFactory loggerFactory, IEventPublisher pub, IEventConsumer con)
        {
            _logger = loggerFactory.CreateLogger<ValuesController>();
            _con = con;
            _pub = pub;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            dynamic jsonObject = new JObject();
            jsonObject.Date = DateTime.Now;
            jsonObject.Album = "Me Against the world";
            jsonObject.Year = 1995;
            jsonObject.Artist = "2Pac";
            var result = new string[] { "value1", "value2" };
            await _pub.PublishAsync(jsonObject);
            return Ok(result);

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _con.ConsumeAsync(new Queue(Startup.AppSettings.Rabbit.QueueName, false));
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
