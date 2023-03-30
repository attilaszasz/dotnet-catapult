using Dummy;
using Types;

namespace WeatherForecastService
{
    public class WeatherForecastService
    {
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(int days)
        {
            //Note: service is tightly coupled to dummy supplier
            var supplier = new DummyWeatherSupplier();
            return await supplier.GetWeatherForecast(days);
        }
    }
}