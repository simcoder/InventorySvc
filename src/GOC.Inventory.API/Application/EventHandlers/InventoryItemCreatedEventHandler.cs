using System.Threading.Tasks;
using GOC.Inventory.API.Application.Helpers;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.Domain;
using GOC.Inventory.Domain.Events;
using Newtonsoft.Json.Linq;

namespace GOC.Inventory.API.Application.EventHandlers
{
    public class InventoryItemCreatedEventHandler : IHandle<InventoryItemCreated>
    {
        private readonly IEventPublisher _eventPub;

        public InventoryItemCreatedEventHandler(IEventPublisher eventPub)
        {
            _eventPub = eventPub;
        }


        public async Task HandleAsync(InventoryItemCreated args)
        {
            var serializedMessage = GocJsonHelper.SerializeJson(args);
            await _eventPub.PublishAsync(JObject.FromObject(args));
        }
    }
}
