using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SaasKit.Multitenancy;

namespace nats_cache_test
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private ILoggerFactory _loggerFactory;
        public Startup(
            ILoggerFactory loggerFactory
        ) {
            _loggerFactory = loggerFactory;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMultitenancy<Tenant, CachingResolver>();
            // services.AddSingleton();
            services.AddMemoryCache();
            
            var sp = services.BuildServiceProvider();
            services.AddSingleton(new EventService(sp.GetService<IMemoryCache>(),  _loggerFactory.CreateLogger(typeof(EventService))));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultitenancy<Tenant>();

            app.UseMvc();
        }
    }
}
