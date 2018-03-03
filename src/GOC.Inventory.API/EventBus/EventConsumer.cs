﻿using System;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using GOC.Inventory.API.Interfaces;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.EventBus
{
    public class EventConsumer : IEventConsumer
    {
        readonly IAdvancedBus _bus;
        readonly ILogger _logger;
        readonly IMessageRouter _messageRouter;

        public EventConsumer(IAdvancedBus bus, ILoggerFactory loggerFactory, IMessageRouter messageRouter)
        {
            _bus = bus;
            _logger = loggerFactory.CreateLogger("EventConsumer");
            _messageRouter = messageRouter;
        }

        public bool IsMessageBusAlive { get { return _bus.IsConnected; }}

        public async Task ConsumeAsync(IQueue queue) => await Task.Factory.StartNew(() =>
                                                      {
                                                         _bus.Consume(queue, (body, properties, info) => Task.Factory.StartNew(() =>
                                                         {
                                                            var message = Encoding.UTF8.GetString(body);
                                                             _messageRouter.RouteAsync(message);             

                                                         }).ContinueWith(task =>
                                                         {
                                                             if (task.IsCompletedSuccessfully)
                                                             {
                                                                 _logger.LogInformation($"Inventory Message succesfully Consumed");
                                                             }
                                                             else
                                                             {
                                                                 _logger.LogWarning($"Inventory Message Consumer was not succesful");
                                                                 throw new EasyNetQException();
                                                             }
                                                         }));
                                                      });
        public void Consume (IQueue queue)
        {
            throw new NotImplementedException();
        }
    }
}

