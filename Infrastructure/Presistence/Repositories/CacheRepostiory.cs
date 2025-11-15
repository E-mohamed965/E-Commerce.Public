using DomainLayer.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    internal class CacheRepostiory(IConnectionMultiplexer _connection) : ICacheRepostiory
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<string?> GetAsync(string CacheKey)
        {
            var cacheValue = await _database.StringGetAsync(CacheKey);
            return cacheValue.IsNullOrEmpty ? null : cacheValue.ToString();
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive)
        {
          await _database.StringSetAsync(CacheKey, CacheValue, TimeToLive);
        }
    }
}
