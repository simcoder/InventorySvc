using System;
using System.Threading.Tasks;
using EasyNetQ.Topology;

namespace GOC.Inventory.API.Interfaces
{
    public interface IEventConsumer
    {
        Task ConsumeAsync(IQueue queue);
    }
}
