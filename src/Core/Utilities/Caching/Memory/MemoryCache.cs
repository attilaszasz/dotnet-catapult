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

        public bool Contains(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return _cache.Contains(key.ToUpperInvariant());
        }

        public void Flush()
        {
            var allKeys = _cache.Select(o => o.Key);
            Parallel.ForEach(allKeys, key => _cache.Remove(key));
        }

        public T? Get<T>(string key) where T : class
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            T value;
            try
            {
                value = (T)_cache.Get(key.ToUpperInvariant());
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

        public void Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _cache.Remove(key.ToUpperInvariant());
        }

        public void Set(string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy());
        }

        public void Set(string key, object value, TimeSpan timeout)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.Add(timeout) });
        }

        public List<KeyValuePair<string, string>> GetCacheContent()
        {
            var cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)_cache).GetEnumerator();
            var results = new List<KeyValuePair<string, string>>();
            while (cacheEnumerator.MoveNext())
            {
                if (!cacheEnumerator.Key.ToString()!.StartsWith("MetadataPrototypes", StringComparison.InvariantCulture))
                    results.Add(new KeyValuePair<string, string>(cacheEnumerator.Key.ToString()!, cacheEnumerator.Value!.ToString()!));
            }
            return results.OrderBy(r => r.Key).ToList();
        }

        public List<string> GetCacheKeys()
        {
            var cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)_cache).GetEnumerator();
            var results = new List<string>();
            while (cacheEnumerator.MoveNext())
            {
                if (!cacheEnumerator.Key.ToString()!.StartsWith("MetadataPrototypes", StringComparison.InvariantCulture))
                    results.Add(cacheEnumerator.Key.ToString()!);
            }
            return results.OrderBy(r => r).ToList();
        }

        public string GetAsString(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            object value;
            try
            {
                value = _cache.Get(key.ToUpperInvariant());
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

        public void Set(string key, string value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy());
        }

        public void Set(string key, string value, TimeSpan timeout)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _cache.Set(key.ToUpperInvariant(), value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.Add(timeout) });
        }
    }

}