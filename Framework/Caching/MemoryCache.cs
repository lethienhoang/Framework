using Microsoft.Extensions.Caching.Memory;
using System;

namespace Framework.Caching
{
    public class MemoryCache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(object key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(object key, T value)
        {
            var absoluteExpirationRelativeFromNow = new TimeSpan(0, 5, 0);
            _memoryCache.Set(key, value, absoluteExpirationRelativeFromNow);
        }
    }
}
