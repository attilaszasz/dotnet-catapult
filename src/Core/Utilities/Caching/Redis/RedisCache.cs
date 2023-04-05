using Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Redis
{
    public class RedisCache : ICache
    {
        private readonly ConnectionMultiplexer _redis;

        private IDatabase Cache => _redis.GetDatabase();

        public RedisCache(ConfigurationOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _redis = ConnectionMultiplexer.Connect(options);
        }

        public async Task<bool> Contains(string key)
        {
            return await Cache.KeyExistsAsync(key.ToUpperInvariant());
        }

        public async Task Flush()
        {
            foreach (var endpoint in _redis.GetEndPoints())
            {
                var server = _redis.GetServer(endpoint);
                await server.FlushDatabaseAsync(flags: CommandFlags.FireAndForget);
            }
        }

        public async Task<T?> Get<T>(string key) where T : class
        {
            var value = await Cache.StringGetAsync(key.ToUpperInvariant());
            return string.IsNullOrWhiteSpace(value.ToString()) ? null : JsonConvert.DeserializeObject<T>(value.ToString());
        }

        public async Task Remove(string key)
        {
            await Cache.KeyDeleteAsync(key.ToUpperInvariant(), CommandFlags.FireAndForget);
        }

        public async Task Set(string key, object value)
        {
            await Cache.StringSetAsync(key.ToUpperInvariant(), JsonConvert.SerializeObject(value), flags: CommandFlags.FireAndForget);
        }

        public async Task Set(string key, object value, TimeSpan timeout)
        {
            await Cache.StringSetAsync(key.ToUpperInvariant(), JsonConvert.SerializeObject(value), timeout, flags: CommandFlags.FireAndForget);
        }

        public Task<List<KeyValuePair<string, string>>> GetCacheContent()
        {
            throw new NotImplementedException("It is not possible to get the complete content of Redis cache.");
        }

        public async Task<List<string>> GetCacheKeys()
        {
            return await Task.Run(() =>
            {
                var server = _redis.GetServer(_redis.GetEndPoints().FirstOrDefault());
                return server.Keys().Select(key => key.ToString()).OrderBy(r => r).ToList();
            });
        }

        public async Task<string> GetAsString(string key)
        {
            var value = await Cache.StringGetAsync(key.ToUpperInvariant());
            return value.ToString();
        }

        public async Task Set(string key, string value)
        {
            await Cache.StringSetAsync(key.ToUpperInvariant(), value, flags: CommandFlags.FireAndForget);
        }

        public async Task Set(string key, string value, TimeSpan timeout)
        {
            await Cache.StringSetAsync(key.ToUpperInvariant(), value, timeout, flags: CommandFlags.FireAndForget);
        }
    }

}
