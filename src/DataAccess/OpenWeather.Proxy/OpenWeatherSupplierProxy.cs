using Dapr.Client;
using Interfaces;
using Types;

namespace OpenWeather.Proxy
{
    public class OpenWeatherSupplierProxy : IWeatherSupplier
    {
        private readonly DaprClient _client;
        private readonly ILogger _logger;

        public OpenWeatherSupplierProxy(DaprClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria)
        {
            try
            {
                return await _client.InvokeMethodAsync<WeatherForecastCriteria, IEnumerable<WeatherForecast>>(HttpMethod.Post, Constants.Suppliers.OpenWeather, "GetWeatherForecast", criteria);
            }
            catch (InvocationException ex)
            {
                _logger.Error(ex, "There was an error invoking OpenWeather API");
                throw;
            }
        }
    }
}