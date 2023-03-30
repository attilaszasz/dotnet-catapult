using Interfaces;
using Microsoft.Extensions.Configuration;
using OpenWeatherMap.Standard;
using OpenWeatherMap.Standard.Enums;
using Types;

namespace OpenWeather
{
    public class OpenWeatherSupplier : IWeatherSupplier
    {
        public static string Name => "OpenWeather";

        private readonly string _apiToken;
        private readonly Current _openWeather;

        public OpenWeatherSupplier(IConfiguration configuration)
        {
            _apiToken = configuration.GetSection("OpenWeatherAPIToken").Value ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_apiToken)) throw new NullReferenceException("Please set the OpenWeatherAPIToken user secret");

            _openWeather = new Current(_apiToken, WeatherUnits.Metric);
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria)
        {
            var results = await _openWeather.GetForecastDataByCoordinatesAsync(criteria.Latitude, criteria.Longitude);

            return results
                .WeatherData
                .Where(w => w.AcquisitionDateTime > DateTime.Today.AddHours(23).AddMinutes(59) && w.AcquisitionDateTime <= DateTime.Today.AddDays(criteria.Days))
                .ToList()
                .ConvertAll(w => new WeatherForecast
                {
                    Date = w.AcquisitionDateTime,
                    TemperatureC = (int)w.WeatherDayInfo.Temperature,
                    Summary = w.Weathers?.FirstOrDefault()?.Description ?? string.Empty
                });
        }
    }
}