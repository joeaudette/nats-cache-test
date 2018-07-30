using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SaasKit.Multitenancy;

namespace nats_cache_test
{
    public class CachingResolver : MemoryCacheTenantResolver<Tenant>
    {
        public CachingResolver(
            IMemoryCache cache,
            ILoggerFactory loggerFactory
        ) : base(cache, loggerFactory)
        {

        }
        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Headers.FirstOrDefault(s => s.Key.Equals("X-Tenant-Id")).Value;
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Tenant> context)
        {
            return new List<string> { context.Tenant.Id };
        }

        protected override Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            var tenantId = GetContextIdentifier(context);
            return Task.Run(() => {
                return new TenantContext<Tenant>(new Tenant { Id = tenantId }); });
        }
    }
}