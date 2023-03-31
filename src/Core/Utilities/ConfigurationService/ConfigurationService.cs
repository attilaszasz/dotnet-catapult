using Interfaces;
using Microsoft.Extensions.Configuration;

namespace ConfigurationService
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigurationService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public T Get<T>(string name) where T : new()
        {
            var instance = new T();
            _configuration.Bind(name, instance);
            return instance;
        }

        public string GetString(string name)
        {
            return _configuration[name] ?? string.Empty;
        }
    }
}