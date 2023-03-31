using Microsoft.Extensions.Configuration;
using OpenWeatherMap.Standard.Enums;
using OpenWeatherMap.Standard.Models;
using OpenWeatherMap.Standard;

namespace OpenWeather
{
    public class OpenWeatherAdapter : IOpenWeatherAdapter
    {
        private readonly string _apiToken;

        private readonly Current _supplier;

        public OpenWeatherAdapter(IConfiguration configuration)
        {
            _apiToken = configuration.GetSection("OpenWeatherAPIToken").Value ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_apiToken)) throw new NullReferenceException("Please set the OpenWeatherAPIToken user secret");

            _supplier = new Current(_apiToken, WeatherUnits.Metric);
        }

        public async Task<ForecastData> GetForecastDataByCoordinatesAsync(double latitude, double longitude)
        {
            return await _supplier.GetForecastDataByCoordinatesAsync(latitude, longitude);
        }
    }
}
