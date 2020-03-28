using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.PostgreSQL
{
    public static class Extensions
    {
        public static DbOptions PostgreOptions(this IServiceCollection services)
        {
            string SectionName = "ConnectionStrings";

            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<DbOptions>(SectionName);
            services.AddSingleton(options);

            return options;
        }
    }
}
