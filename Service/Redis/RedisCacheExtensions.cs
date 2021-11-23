using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Redis
{
    public class RedisCacheExtensions
    {
        private readonly IDistributedCache distributedCache;

        private readonly DistributedCacheEntryOptions distributedCacheEntryOptions  =
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(2)
            };

        public RedisCacheExtensions(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, System.Threading.CancellationToken token = default)
        {
            await this.distributedCache.SetStringAsync(key, value, options ?? distributedCacheEntryOptions, token);
        }

        public async Task SetStringAsync(string key, string value, System.Threading.CancellationToken token = default)
        {
            await this.distributedCache.SetStringAsync(key, value, distributedCacheEntryOptions, token);
        }

        public async Task<string> GetStringAsync(string key, System.Threading.CancellationToken token = default)
        {
            return await this.distributedCache.GetStringAsync(key, token);
        }
    }
}
