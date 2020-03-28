using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.CosmosDb
{
    public static class Extensions
    {
        private static readonly string SectionName = "CommosDb";

        public static void AddCommosDBService(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<DbOptions>(SectionName);

            var serviceEndpoint = new Uri(options.EndpointUrl);
            var connectionPolicy = new ConnectionPolicy();           

            var client = new DocumentClient(serviceEndpoint, options.AuthKey, connectionPolicy);

            services.AddSingleton<IDocumentClient>(sp =>
            {
                return client;
            });
        }
    }
}
