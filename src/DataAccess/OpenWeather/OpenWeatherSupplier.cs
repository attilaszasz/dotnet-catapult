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

        //NOTE: because the OpenWeatherMap library does not follow intarface based programming model,
        private readonly IOpenWeatherAdapter _api;

        public OpenWeatherSupplier(IOpenWeatherAdapter api)
        {
            _api = api;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria)
        {
            var results = await _api.GetForecastDataByCoordinatesAsync(criteria.Latitude, criteria.Longitude);

            return results
                .WeatherData?
                .Where(w => w.AcquisitionDateTime > DateTime.Today.AddHours(23).AddMinutes(59) && w.AcquisitionDateTime <= DateTime.Today.AddDays(criteria.Days))
                .ToList()
                .ConvertAll(w => new WeatherForecast
                {
                    Date = w.AcquisitionDateTime,
                    TemperatureC = (int)w.WeatherDayInfo.Temperature,
                    Summary = w.Weathers?.FirstOrDefault()?.Description ?? string.Empty
                }) 
                ?? Enumerable.Empty<WeatherForecast>();
        }

    }
}