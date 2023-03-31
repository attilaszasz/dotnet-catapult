using Helpers;
using Interfaces;
using Types;

namespace WeatherForecastService
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IEnumerable<Lazy<IWeatherSupplier, SupplierMetadata>> _suppliers;
        private readonly ILogger _logger;

        public WeatherForecastService(IEnumerable<Lazy<IWeatherSupplier, SupplierMetadata>> suppliers, ILogger logger)
        {
            _suppliers = suppliers;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria, string supplierName)
        {
            //NOTE: getting the supplier by metadata, from the lazily injected collection
            var supplier = _suppliers.GetSupplier(supplierName);

            _logger.Information("Requesting weather forecast from {supplier} supplier with {@criteria}", supplierName, criteria);

            return await supplier.GetWeatherForecast(criteria);
        }
    }
}