using System;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using GOC.Inventory.API.Interfaces;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.EventBus
{
    public class EventPublisher<T> : IDisposable, IEventPublisher<T> where T : class
    {
        readonly IAdvancedBus _bus;
        readonly IQueue _queue;
        readonly IExchange _exchange;
        readonly ILogger _logger;

        public EventPublisher(IAdvancedBus bus, ILoggerFactory loggerFactory)
        {
            _bus = bus;

            _logger = loggerFactory.CreateLogger("EventPublisher");

            // create a topic exchange
            _exchange = _bus.ExchangeDeclare(Startup.AppSettings.Rabbit.ExchangeName, ExchangeType.Topic);

            // declare a durable queue
            _queue = _bus.QueueDeclare(Startup.AppSettings.Rabbit.QueueName);

            // bind queue to exchange
            _bus.Bind(_exchange, _queue, routingKey: Startup.AppSettings.Rabbit.RoutingKey);
        }

        public void Dispose()
        {
            _logger.LogDebug("Disposing");
            _bus.SafeDispose();
        }

        public async Task PublishAsync (T message)
        {
            var msg = new Message<T>(message);
            msg.Properties.AppId = "InventoryService";
            var g = Exchange.GetDefault();
            await _bus.PublishAsync(_exchange, Startup.AppSettings.Rabbit.RoutingKey, true, msg)
                      .ContinueWith(task =>
            {
                // this only checks that the task finished
                // IsCompleted will be true even for tasks in a faulted state
                // we use if (task.IsCompleted && !task.IsFaulted) to check for success
                if (task.IsCompleted)
                {
                    _logger.LogInformation($"Inventory Message succesfully Published");
                }
                if (task.IsFaulted)
                {
                    _logger.LogWarning($"Inventory Message Publish was not succesful");
                }
            });
        } 
    }
}
