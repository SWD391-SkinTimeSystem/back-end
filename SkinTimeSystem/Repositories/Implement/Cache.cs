using Newtonsoft.Json;
using Repositories.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class Cache : ICache
    {
        private readonly IDatabase _redis;
        public Cache(IDatabase redis)
        {
            _redis = redis;
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) => await _redis.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry);

        public async Task<T> GetAsync<T>(string key) => JsonConvert.DeserializeObject<T>(await _redis.StringGetAsync(key)) ?? default;


        public async Task<bool> DeleteAsync<T>(string key) => await _redis.KeyDeleteAsync(key);


    }
}
