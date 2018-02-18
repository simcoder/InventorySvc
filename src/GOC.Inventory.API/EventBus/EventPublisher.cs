using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using GOC.Inventory.API.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GOC.Inventory.API.EventBus
{
    public class EventPublisher : IEventPublisher
    {
        readonly IAdvancedBus _bus;
        readonly IExchange _exchange;
        readonly ILogger _logger;

        //constructor
        public EventPublisher(IAdvancedBus bus, ILoggerFactory loggerFactory)
        {
            _bus = bus;
            _logger = loggerFactory.CreateLogger("EventPublisher");

            // create a topic exchange
            _exchange = _bus.ExchangeDeclare(Startup.AppSettings.Rabbit.ExchangeName, ExchangeType.Fanout);
            // declare a durable queue
            foreach(var queue in Startup.AppSettings.Rabbit.PublishingQueues)
            {
                // declare queues
                _bus.QueueDeclare(queue.Name);
                // bind queues
                _bus.Bind(_exchange, new Queue(queue.Name, false), routingKey: string.Empty);

            }
        }

        /// <summary>
        /// Publishes async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        public async Task PublishAsync (string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            await publishMessageAsync(body, _exchange);
        }

        /// <summary>
        /// Publishs async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        public async Task PublishAsync(JObject message)
        {
            var body = Encoding.UTF8.GetBytes(message.ToString());

            await publishMessageAsync(body, _exchange);
        }

        private async Task publishMessageAsync(byte[] body, IExchange exchange,string routingKey = "")
        {
            await _bus.PublishAsync(exchange, routingKey, true, new MessageProperties(), body)
                      .ContinueWith(task =>
                      {
                          // this only checks that the task finished
                          if (task.IsCompleted)
                          {
                              _logger.LogInformation($"Inventory Message succesfully Published");
                          }
                          if (task.IsFaulted)
                          {
                              _logger.LogWarning($"Error Publish was not succesful");
                          }
                      });
        }
    }
}
