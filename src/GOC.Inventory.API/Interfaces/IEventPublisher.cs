using System;
using System.Threading.Tasks;

namespace GOC.Inventory.API.Interfaces
{
    public interface IEventPublisher<T> : IDisposable
    {
        Task PublishAsync(T message);
    }
}
