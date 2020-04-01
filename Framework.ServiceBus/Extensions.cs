using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.ServiceBus
{
    public static class Extensions
    {
        private static readonly string SectionName = "AzServiceBus";

        public static void AddMessageServiceBus(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<SBQOptions>(SectionName);

            var client = new QueueClient(options.Endpoint, options.QueueName);

            services.AddSingleton<IQueueClient>(sp =>
            {
                return client;
            });
        }
    }
}
