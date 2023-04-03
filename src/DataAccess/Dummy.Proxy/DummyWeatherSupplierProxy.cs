using Dapr.Client;
using Interfaces;
using Types;

namespace Dummy.Proxy
{
    public class DummyWeatherSupplierProxy : IWeatherSupplier
    {
        private readonly DaprClient _client;
        private readonly ILogger _logger;

        public DummyWeatherSupplierProxy(DaprClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria)
        {
            try
            {
                return await _client.InvokeMethodAsync<WeatherForecastCriteria, IEnumerable<WeatherForecast>>(HttpMethod.Post, "Dummy", "GetWeatherForecast", criteria);
            }
            catch (InvocationException ex)
            {
                _logger.Error(ex, "There was an error invoking Dummy API");
                throw;
            }
        }
    }
}
