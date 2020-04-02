using Framework.HTTP.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Framework.HTTP
{
    public static class Extensions
    {
        public static void RegisterNamedHttpClientFactories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            var clients = configuration.GetSection("NamedHttpClientFactories").Get<NamedHttpClientFactories[]>();

            foreach(var namedHttpClient in clients)
            {
                services.AddHttpClient(namedHttpClient.Name, (serviceProvider, config) =>
                {
                    config.BaseAddress = new System.Uri(namedHttpClient.Address);
                    config.DefaultRequestHeaders.Add("Accept", "application/json");

                    var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                    if (httpContextAccessor.HttpContext == null)
                    {
                        return;
                    }

                    var bearerToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"]
                        .FirstOrDefault(x => x.StartsWith("bearer", StringComparison.InvariantCultureIgnoreCase));
                    if (!string.IsNullOrEmpty(bearerToken))
                    {
                        config.DefaultRequestHeaders.Add("Authorization", bearerToken);
                    }
                });
            }
        }

        public static void AddHttpClientFactoyExtension(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClient, HttpClientExtensions>();
        }
    }
}
