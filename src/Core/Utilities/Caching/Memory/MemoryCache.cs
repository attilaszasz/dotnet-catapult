using Interfaces;
using System.Collections;
using System.Runtime.Caching;

namespace Memory
{
    public class MemoryCache : ICache
    {
        private readonly System.Runtime.Caching.MemoryCache _cache;

        public MemoryCache(System.Runtime.Caching.MemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<bool> Contains(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return await Task.FromResult(_cache.Contains(key.ToUpperInvariant()));
        }

        public async Task Flush()
        {
            var allKeys = _cache.Select(o => o.Key);
            await Task.WhenAll(allKeys.Select(key => Task.Run(() => _cache.Remove(key))));

        }

        public async Task<T?> Get<T>(string key) where T : class
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            T value;
            try
            {
                value = await Task.FromResult((T)_cache.Get(key.ToUpperInvariant()));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (NotSupportedException)
            {
                throw;
            }
            return value;
        }

        public async Task Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            await Task.Run(() => _cache.Remove(key.ToUpperInvariant()));
        }

        public async Task Set(string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            await Task.Run(() => _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy()));
        }

        public async Task Set(string key, object value, TimeSpan timeout)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            await Task.Run(() => _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.Add(timeout) }));
        }

        public async Task<List<KeyValuePair<string, string>>> GetCacheContent()
        {
            return await Task.Run(() =>
            {
                var cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)_cache).GetEnumerator();
                var results = new List<KeyValuePair<string, string>>();
                while (cacheEnumerator.MoveNext())
                {
                    if (!cacheEnumerator.Key.ToString()!.StartsWith("MetadataPrototypes", StringComparison.InvariantCulture))
                        results.Add(new KeyValuePair<string, string>(cacheEnumerator.Key.ToString()!, cacheEnumerator.Value!.ToString()!));
                }
                return results.OrderBy(r => r.Key).ToList();
            });
        }

        public async Task<List<string>> GetCacheKeys()
        {
            return await Task.Run(() =>
            {
                var cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)_cache).GetEnumerator();
                var results = new List<string>();
                while (cacheEnumerator.MoveNext())
                {
                    if (!cacheEnumerator.Key.ToString()!.StartsWith("MetadataPrototypes", StringComparison.InvariantCulture))
                        results.Add(cacheEnumerator.Key.ToString()!);
                }
                return results.OrderBy(r => r).ToList(); 
            });
        }

        public async Task<string> GetAsString(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            object value;
            try
            {
                value = await Task.FromResult(_cache.Get(key.ToUpperInvariant()));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (NotSupportedException)
            {
                throw;
            }
            return value?.ToString() ?? string.Empty;
        }

        public async Task Set(string key, string value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            await Task.Run(() => _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy()));
        }

        public async Task Set(string key, string value, TimeSpan timeout)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            await Task.Run(() => _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.Add(timeout) }));
        }
    }

}