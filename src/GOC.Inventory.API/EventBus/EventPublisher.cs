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
        readonly IExchange _errorErrorExchange;
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
            foreach(ConsumerQueue queue in Startup.AppSettings.Rabbit.ConsumerQueues)
            {
                // declare queues
                _bus.QueueDeclare(queue.Name);
                // bind queues
                _bus.Bind(_exchange, new Queue(queue.Name, false), routingKey: string.Empty);

            }
            // declare error exchange and bind to error queue
            _errorErrorExchange = _bus.ExchangeDeclare(Startup.AppSettings.Rabbit.ErrorExchangeName, ExchangeType.Topic);
            _bus.QueueDeclare(Startup.AppSettings.Rabbit.ErrorQueueName);
            _bus.Bind(_errorErrorExchange, new Queue(Startup.AppSettings.Rabbit.ErrorQueueName, false), "IS.*");

        }

        /// <summary>
        /// Publishes async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        /// <param name="isError">If set to <c>true</c> is error.</param>
        public async Task PublishAsync (string message, bool isError)
        {
            var body = Encoding.UTF8.GetBytes(message);
            if (isError)
            {
                await publishMessageAsync(body, _errorErrorExchange, true ,"IS.*");
            }
            else
            {
                await publishMessageAsync(body, _exchange, false);
            }

        }

        /// <summary>
        /// Publishs async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        /// <param name="isError">If set to <c>true</c> is error.</param>
        public async Task PublishAsync(JObject message, bool isError)
        {
            var body = Encoding.UTF8.GetBytes(message.ToString());
            if (isError)
            {
                await publishMessageAsync(body, _errorErrorExchange, true, "IS.*");
            }
            else
            {
                await publishMessageAsync(body, _exchange, false);
            }
        }

        private async Task publishMessageAsync(byte[] body, IExchange exchange, bool isError ,string routingKey = "")
        {
            await _bus.PublishAsync(exchange, routingKey, true, new MessageProperties(), body)
                      .ContinueWith(task =>
                      {
                          // this only checks that the task finished
                          if (task.IsCompleted)
                          {
                               if(isError)
                               {
                                  _logger.LogInformation($"Error Published from Inventory Service");
                               }
                               else
                               {
                                  _logger.LogInformation($"Inventory Message succesfully Published");
                               }
                          }
                          if (task.IsFaulted)
                          {
                              if (isError)
                              {
                                  _logger.LogWarning($"Error Publish was not succesful");
                              }
                              else
                              {
                                  _logger.LogInformation($"Error Published from Inventory Service");
                              }
                          }
                      });
        }
    }
}
