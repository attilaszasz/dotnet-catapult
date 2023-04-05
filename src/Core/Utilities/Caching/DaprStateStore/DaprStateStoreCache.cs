using Dapr.Client;
using Interfaces;

namespace DaprStateStore
{
    public class DaprStateStoreCache : ICache
    {
        private readonly DaprClient _client;

        public DaprStateStoreCache(DaprClient client)
        {
            _client = client;
        }

        public Task<bool> Contains(string key)
        {
            throw new NotImplementedException();
        }

        public Task Flush()
        {
            throw new NotImplementedException();
        }

        public async Task<T?> Get<T>(string key) where T : class
        {
            return await _client.GetStateAsync<T>(Types.Constants.StateStoreName, key);
        }

        public async Task<string> GetAsString(string key)
        {
            return await _client.GetStateAsync<string>(Types.Constants.StateStoreName, key);
        }

        public Task<List<KeyValuePair<string, string>>> GetCacheContent()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetCacheKeys()
        {
            throw new NotImplementedException();
        }

        public async Task Remove(string key)
        {
            await _client.DeleteStateAsync(Types.Constants.StateStoreName, key);
        }

        public async Task Set(string key, string value)
        {
            await _client.SaveStateAsync(Types.Constants.StateStoreName, key, value);
        }

        public async Task Set(string key, string value, TimeSpan timeout)
        {
            await _client.SaveStateAsync(Types.Constants.StateStoreName, key, value, metadata: new Dictionary<string, string>() { { "ttlInSeconds", timeout.TotalSeconds.ToString() } });
        }

        public async Task Set(string key, object value)
        {
            await _client.SaveStateAsync(Types.Constants.StateStoreName, key, value);
        }

        public async Task Set(string key, object value, TimeSpan timeout)
        {
            await _client.SaveStateAsync(Types.Constants.StateStoreName, key, value, metadata: new Dictionary<string, string>() { { "ttlInSeconds", timeout.TotalSeconds.ToString() }});
        }
    }
}