using System;
using System.Threading.Tasks;

namespace GOC.Inventory.API.Interfaces
{
    public interface IMessageRouter
    {
        Task RouteAsync(string message);
    }
}
