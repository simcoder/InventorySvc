using System.Collections.Generic;
using Microphone.Consul;

namespace GOC.Inventory.API
{
    public class AppSettings
    {
        public ConsulOptions Consul { get; set; }
        public IdentitySettings Identity { get; set; }
        public RabbitMQSettings Rabbit { get; set; }
        public PostGres PostGres { get; set; }

    }
    public class IdentitySettings
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
    }
    public class RabbitMQSettings
    {
        public string Host { get; set; }
        public string PublisherConfirms { get; set; }
        public string Timeout { get; set; }
        public IList<PublishingQueue> PublishingQueues { get; set; } 
        public string ConsumableQueue { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
    }
    public class PublishingQueue
    {
        public string Name { get; set; }
    }
    public class PostGres
    {
        public string ConnectionString { get; set; }
    }
}
