
using Microsoft.Extensions.DependencyInjection;

namespace Framework.CQRS.Dispatchers
{
    public static class Extensions
    {
        public static void AddDispatchers(this IServiceCollection services)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IDispatcher, Dispatcher>();
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        }
    }
}
