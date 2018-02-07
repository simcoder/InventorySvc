using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GOC.Inventory.API.Interfaces
{
    public interface IEventPublisher : IDisposable
    {
        Task PublishAsync(JObject message);
    }
}
