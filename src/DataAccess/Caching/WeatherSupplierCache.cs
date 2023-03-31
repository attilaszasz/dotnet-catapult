using Interfaces;
using Types.Caching;
using Types;

namespace Caching
{
    public class WeatherSupplierCache : IWeatherSupplier
    {
        private readonly IWeatherSupplier _supplier;
        private readonly ICache _cache;
        private readonly IConfigurationService _configurationService;
        private readonly CacheSettings _cacheSettings;
        private readonly ILogger _logger;

        public WeatherSupplierCache(IWeatherSupplier supplier, ICache cache, IConfigurationService configurationService, ILogger logger)
        {
            _supplier = supplier;
            _cache = cache;
            _configurationService = configurationService;
            _cacheSettings = _configurationService.Get<CacheSettings>("Caching");
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria)
        {
            var keys = string.Format(CacheKeys.GetWeatherForecast, criteria.Longitude, criteria.Latitude, criteria.Days);

            var result = _cache.Get<IEnumerable<WeatherForecast>>(keys);

            if (result != null)
            {
                _logger.Information("Found {key} in {cache}", keys, _cache.GetType().ToString());
                return result;
            }

            _logger.Information("{key} in {cache} was NOT found.", keys, _cache.GetType().ToString());

            result = await _supplier.GetWeatherForecast(criteria);

            _logger.Information("Setting {key} in {cache} for {duration}s.", keys, _cache.GetType().ToString(), _cacheSettings.Duration.Short);
            if (result != null) _cache.Set(keys, result, TimeSpan.FromSeconds(_cacheSettings.Duration.Short));  //Caching weather data for 10 mins

            return result ?? Enumerable.Empty<WeatherForecast>();

        }
    }
}
