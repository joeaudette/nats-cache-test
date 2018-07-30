using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SaasKit.Multitenancy;

namespace nats_cache_test.Controllers {
    public class CacheController : Controller {
        private IMemoryCache _cache;
        private EventService _eventSvc;

        public CacheController(
            IMemoryCache cache,
            EventService eventSvc
        ) {
            _cache = cache;
            _eventSvc = eventSvc;
        }

        [HttpGet("/{id}/cached")]
        public IActionResult IsCache(string id) {
            TenantContext<Tenant> tenant;
            var isCached = _cache.TryGetValue(id, out tenant);
            return Ok(isCached);
        }

        [HttpGet("/{id}/purge")]
        public IActionResult PurgeItem(string id) {
            _eventSvc.Nats.Publish("test", Encoding.UTF8.GetBytes("foo"));
            return Ok();
        } 
    }
}