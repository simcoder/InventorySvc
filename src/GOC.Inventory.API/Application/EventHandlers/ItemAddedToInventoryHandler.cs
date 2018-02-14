using System.Threading.Tasks;
using EasyNetQ;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.Domain.Events;
using Newtonsoft.Json.Linq;

namespace GOC.Inventory.Domain.Handlers
{
    public class NotifyNewItemAddedToInventory : IHandle<InventoryItemAdded>
    {
        private readonly IEventPublisher _eventPub;

        public NotifyNewItemAddedToInventory(IEventPublisher eventPub)
        {
            _eventPub = eventPub;
        }


        public async Task HandleAsync(InventoryItemAdded args)
        {
           // await _eventPub.PublishAsync(JObject.FromObject(args), false);
        }
    }
}
