using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ucl.PontoNet.Api.Filters
{
    public class MemoryCacheWithPolicy<UserInfoDto> : IMemoryCacheWithPolicy<UserInfoDto>
    {
        private IMemoryCache _cache { get; set; }
        /*private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 5000
        });*/

        public MemoryCacheWithPolicy(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<UserInfoDto> GetOrCreateAsync(object key, Func<Task<UserInfoDto>> createItem)
        {
            UserInfoDto cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                cacheEntry = await createItem();

                if (cacheEntry == null)
                {
                    return default(UserInfoDto);
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                 .SetSize(1)
                    .SetPriority(CacheItemPriority.High)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));


                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }
            return cacheEntry;
        }
    }
}
