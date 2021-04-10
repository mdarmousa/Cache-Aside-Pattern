using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache_Aside_Pattern.Cache
{

    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<string, TimeSpan> _expirationConfiguration;

        public MemoryCacheService(
            IMemoryCache memoryCache,
            Dictionary<string, TimeSpan> expirationConfiguration
            )
        {
            _memoryCache = memoryCache;
            _expirationConfiguration = expirationConfiguration;
        }

        public void Add<TItem>(TItem item, string key)
        {
            var cachedObjectName = item.GetType().Name;
            var timespan =  _expirationConfiguration[cachedObjectName];

            _memoryCache.Set(key, item, timespan);
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
