namespace Interfaces
{
    public interface ICache
    {
        bool Contains(string key);
        void Flush();
        T? Get<T>(string key) where T : class;
        void Remove(string key);
        void Set(string key, string value);
        void Set(string key, string value, TimeSpan timeout);
        void Set(string key, object value);
        void Set(string key, object value, TimeSpan timeout);
        List<string> GetCacheKeys();
        List<KeyValuePair<string, string>> GetCacheContent();
        string GetAsString(string key);
    }
}
