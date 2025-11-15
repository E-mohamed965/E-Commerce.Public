using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class CacheService(ICacheRepostiory _cacheRepostiory) : ICacheService
    {
        public async Task<string?> GetAsync(string CacheKey)
        {
           return await _cacheRepostiory.GetAsync(CacheKey);
        }

        public async Task SetAsync(string CacheKey, object CacheValue, TimeSpan TimeToLive)
        {
            var value = JsonSerializer.Serialize(CacheValue);
           await _cacheRepostiory.SetAsync(CacheKey, value, TimeToLive);
        }
    }
}
