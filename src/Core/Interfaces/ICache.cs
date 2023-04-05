namespace Interfaces
{
    public interface ICache
    {
        Task<bool> Contains(string key);
        Task Flush();
        Task<T?> Get<T>(string key) where T : class;
        Task Remove(string key);
        Task Set(string key, string value);
        Task Set(string key, string value, TimeSpan timeout);
        Task Set(string key, object value);
        Task Set(string key, object value, TimeSpan timeout);
        Task<List<string>> GetCacheKeys();
        Task<List<KeyValuePair<string, string>>> GetCacheContent();
        Task<string> GetAsString(string key);
    }
}
