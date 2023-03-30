using Dummy;
using OpenWeather;
using Types;

namespace WeatherForecastService
{
    public class WeatherForecastService
    {
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(double latitude, double longitude, int days, string supplierName)
        {
            //Note: service is tightly coupled to both dummy and openweather suppliers
            if (supplierName == DummyWeatherSupplier.Name)
            {
                var supplier = new DummyWeatherSupplier();
                return await supplier.GetWeatherForecast(days);
            }
            if (supplierName == OpenWeatherSupplier.Name)
            {
                var supplier = new OpenWeatherSupplier();
                return await supplier.GetWeatherForecast(latitude, longitude, days);
            }
            throw new ArgumentException($"Unknown supplier: {supplierName}");
        }
    }
}