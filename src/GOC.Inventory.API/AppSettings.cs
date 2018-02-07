using Microphone.Consul;

namespace GOC.Inventory.API
{
    public class AppSettings
    {
        public ConsulOptions Consul { get; set; }
        public IdentitySettings Identity { get; set; }
        public RabbitMQSettings Rabbit
        {
            get;
            set;
        }
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
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
    }
}
