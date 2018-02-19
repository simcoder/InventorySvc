using Newtonsoft.Json;

namespace GOC.Inventory.API.Application.Helpers
{
    public static class GocJsonHelper
    {
        public static string SerializeJson (object obj)
        {
           return JsonConvert.SerializeObject(obj, Formatting.None,
                       new JsonSerializerSettings()
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });
        }
    }
}
