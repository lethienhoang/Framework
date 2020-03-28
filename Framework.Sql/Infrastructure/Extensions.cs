using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Sql
{
    public static class Extensions
    {
        public static DbOptions SqlOptions(this IServiceCollection services)
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
