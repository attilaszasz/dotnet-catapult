using OpenWeatherMap.Standard.Models;
using OpenWeatherMap.Standard;

namespace OpenWeather
{
    public class OpenWeatherAdapter : IOpenWeatherAdapter
    {
        private readonly Current _supplier;

        public OpenWeatherAdapter(Current supplier)
        {
            _supplier = supplier;
        }

        public async Task<ForecastData> GetForecastDataByCoordinatesAsync(double latitude, double longitude)
        {
            return await _supplier.GetForecastDataByCoordinatesAsync(latitude, longitude);
        }
    }
}
