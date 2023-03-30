using Types;

namespace Interfaces
{
    public interface IWeatherSupplier
    {
        public Task<IEnumerable<WeatherForecast>> GetWeatherForecast(WeatherForecastCriteria criteria);
    }
}