using OpenWeatherMap.Standard.Models;

namespace OpenWeather
{
    public interface IOpenWeatherAdapter
    {
        Task<ForecastData> GetForecastDataByCoordinatesAsync(double latitude, double longitude);
    }
}
