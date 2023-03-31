using Interfaces;
using Types;

namespace WeatherForecastService
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherSupplier _dummy;
        private readonly IWeatherSupplier _openWeather;
        private readonly ILogger _logger;

        public WeatherForecastService(IWeatherSupplier dummy, IWeatherSupplier openWeather, ILogger logger)
        {
            _dummy = dummy;
            _openWeather = openWeather;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria, string supplierName)
        {
            //NOTE: for each future supplier, we'll need to add another if statement here. Also, dependent on magic strings
            if (supplierName == "Dummy")
            {
                _logger.Information("Requesting weather forecast from {supplier} supplier with {@criteria}", supplierName, criteria);
                return await _dummy.GetWeatherForecast(criteria);
            }
            if (supplierName == "OpenWeather")
            {
                _logger.Information("Requesting weather forecast from {supplier} supplier with {@criteria}", supplierName, criteria);
                return await _openWeather.GetWeatherForecast(criteria);
            }
            //NOTE: we can do logging here while running the webapp, because the ILogger is declared in ApiModule. Need to mock in tests that don't register that module.
            _logger.Error($"Unknown supplier: {supplierName}");
            throw new ArgumentException($"Unknown supplier: {supplierName}");
        }
    }
}