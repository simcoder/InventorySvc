using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GOC.Inventory.API.Interfaces
{
    public interface IEventPublisher 
    {
        Task PublishAsync(string message, bool isError);
        Task PublishAsync(JObject message, bool isError);
    }
}
