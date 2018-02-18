using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ.Topology;
using GOC.Inventory.API.Interfaces;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Application.BackgroundServices
{
    public class BackgroundEventSubcriptionService : BackgroundService
    {
        private readonly ILogger _logger;
        IEventConsumer _eventConsumer;
        public BackgroundEventSubcriptionService(ILoggerFactory logger, IEventConsumer eventConsumer)
        {
            _logger = logger.CreateLogger<BackgroundEventSubcriptionService>();
            _eventConsumer = eventConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("polling messages for inventory service");
                await _eventConsumer.ConsumeAsync(new Queue("EasyNetQ_Default_Error_Queue", false));
                await _eventConsumer.ConsumeAsync(new Queue(Startup.AppSettings.Rabbit.ConsumableQueue, false));
                await Task.Delay(TimeSpan.FromHours(8), stoppingToken);
            }
        }
    }
}
