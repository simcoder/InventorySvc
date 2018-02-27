using System;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using GOC.Inventory.API.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Polly;

namespace GOC.Inventory.API.EventBus
{
    public class EventPublisher : IEventPublisher
    {
        readonly IAdvancedBus _bus;
        IExchange Exchange;
        readonly ILogger _logger;

        //constructor
        public EventPublisher(IAdvancedBus bus, ILoggerFactory loggerFactory)
        {
            _bus = bus;
            _logger = loggerFactory.CreateLogger("EventPublisher");
        }

        /// <summary>
        /// Publishes async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        public async Task PublishAsync (string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            await publishMessageAsync(body);
        }

        /// <summary>
        /// Publishs async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        public async Task PublishAsync(JObject message)
        {
            var body = Encoding.UTF8.GetBytes(message.ToString());

            await publishMessageAsync(body);
        }

        private async Task publishMessageAsync(byte[] body, string routingKey = "")
        {
            _logger.LogDebug("publish with retry policy");
            var retryPolicy = Policy
                  .Handle<Exception>()
                .WaitAndRetryForeverAsync(retry => TimeSpan.FromSeconds(5));
            
            await retryPolicy.ExecuteAsync(async () =>
            {
                _logger.LogDebug("retry attempt");

                if (!_bus.IsConnected)
                    throw new Exception("Message bus is not connected");
                // create a topic exchange
                Exchange = _bus.ExchangeDeclare(Startup.AppSettings.Rabbit.ExchangeName, ExchangeType.Fanout);
                // declare a durable queue
                foreach (var queue in Startup.AppSettings.Rabbit.PublishingQueues)
                {
                    // declare queues
                    _bus.QueueDeclare(queue.Name);
                    // bind queues
                    _bus.Bind(Exchange, new Queue(queue.Name, false), routingKey: string.Empty);

                }
                await _bus.PublishAsync(Exchange, routingKey, true, new MessageProperties(), body)
                          .ContinueWith(task =>
                          {
                              if (task.IsCompletedSuccessfully)
                              {
                                  _logger.LogInformation($"Inventory Message succesfully Published");
                              }
                              else
                              {
                                  _logger.LogError($"Error Publish was not succesful");
                                  _logger.LogError(task.Exception.Message);
                                  throw new EasyNetQException();
                              }
                          });
            });

        }
    }
}
