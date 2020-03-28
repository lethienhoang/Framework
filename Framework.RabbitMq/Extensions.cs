using System;

namespace Framework.RabbitMq
{
    public static class Extensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
