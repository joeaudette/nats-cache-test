using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NATS.Client;

namespace nats_cache_test
{
    public class EventService
    {
        public IConnection Nats;

        public EventService(
            IMemoryCache _cache,
            ILogger _logger
        )
        {
            ConnectionFactory factory = new ConnectionFactory();

            Nats = factory.CreateConnection();
            Nats.SubscribeAsync("test", (sender, args) =>
            {
                var id = Encoding.UTF8.GetString(args.Message.Data);

                _logger.LogWarning($"Clearing cache for key {id}");
                _cache.Remove(id);
            }).Start();
        }
    }
}