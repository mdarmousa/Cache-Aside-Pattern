using Microsoft.Extensions.Caching.Memory;
using System;

namespace Cache_Aside_Pattern.Cache
{

    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add<TItem>(string key, TItem item, TimeSpan timeToLeft)
        {

            _memoryCache.Set(key, item, timeToLeft);
        }

        public TItem Get<TItem>(string key) where TItem : class
        {
            if (_memoryCache.TryGetValue(key, out TItem value))
            {
                return value;
            }

            return null;
        }
    }
}
