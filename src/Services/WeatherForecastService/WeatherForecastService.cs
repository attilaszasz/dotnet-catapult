using Dummy;
using Interfaces;
using OpenWeather;
using Types;

namespace WeatherForecastService
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherSupplier _dummy;
        private readonly IWeatherSupplier _openWeather;

        public WeatherForecastService(IWeatherSupplier dummy, IWeatherSupplier openWeather)
        {
            _dummy = dummy;
            _openWeather = openWeather;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria, string supplierName)
        {
            //Note: service is tightly coupled to both dummy and openweather suppliers
            if (supplierName == DummyWeatherSupplier.Name)
            {
                return await _dummy.GetWeatherForecast(criteria);
            }
            if (supplierName == OpenWeatherSupplier.Name)
            {
                return await _openWeather.GetWeatherForecast(criteria);
            }
            throw new ArgumentException($"Unknown supplier: {supplierName}");
        }
    }
}