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

        public bool Contains(string key)
        {
            return Cache.KeyExists(key.ToUpperInvariant());
        }

        public void Flush()
        {
            foreach (var endpoint in _redis.GetEndPoints())
            {
                var server = _redis.GetServer(endpoint);
                server.FlushDatabase(flags: CommandFlags.FireAndForget);
            }
        }

        public T? Get<T>(string key) where T : class
        {
            string value = Cache.StringGet(key.ToUpperInvariant()).ToString();
            return string.IsNullOrWhiteSpace(value) ? null : JsonConvert.DeserializeObject<T>(value);
        }

        public void Remove(string key)
        {
            Cache.KeyDelete(key.ToUpperInvariant(), CommandFlags.FireAndForget);
        }

        public void Set(string key, object value)
        {
            Cache.StringSet(key.ToUpperInvariant(), JsonConvert.SerializeObject(value), flags: CommandFlags.FireAndForget);
        }

        public void Set(string key, object value, TimeSpan timeout)
        {
            Cache.StringSet(key.ToUpperInvariant(), JsonConvert.SerializeObject(value), timeout, flags: CommandFlags.FireAndForget);
        }

        public List<KeyValuePair<string, string>> GetCacheContent()
        {
            throw new NotImplementedException("It is not possible to get the complete content of Redis cache.");
        }

        public List<string> GetCacheKeys()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().FirstOrDefault());
            return server.Keys().Select(key => key.ToString()).OrderBy(r => r).ToList();
        }

        public string GetAsString(string key)
        {
            return Cache.StringGet(key.ToUpperInvariant()).ToString();
        }

        public void Set(string key, string value)
        {
            Cache.StringSet(key.ToUpperInvariant(), value, flags: CommandFlags.FireAndForget);
        }

        public void Set(string key, string value, TimeSpan timeout)
        {
            Cache.StringSet(key.ToUpperInvariant(), value, timeout, flags: CommandFlags.FireAndForget);
        }
    }

}
