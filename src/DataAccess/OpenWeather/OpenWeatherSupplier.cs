using Microsoft.Extensions.Configuration;
using OpenWeatherMap.Standard;
using OpenWeatherMap.Standard.Enums;
using Types;

namespace OpenWeather
{
    public class OpenWeatherSupplier
    {
        public static string Name => "OpenWeather";

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(double latitude, double longitude, int days)
        {
            //See https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows on how to set up local user secrets
            var config = new ConfigurationBuilder().AddUserSecrets("7b91147d-502c-4fa9-b973-294be01c474b").Build();

            string token = config.GetSection("OpenWeatherAPIToken").Value ?? string.Empty;

            if (string.IsNullOrWhiteSpace(token)) throw new NullReferenceException("Please set the OpenWeatherAPIToken user secret");

            var supplier = new Current(token, WeatherUnits.Metric);
            var results = await supplier.GetForecastDataByCoordinatesAsync(latitude, longitude);

            return results
                .WeatherData
                .Where(w => w.AcquisitionDateTime > DateTime.Today.AddHours(23).AddMinutes(59) && w.AcquisitionDateTime < DateTime.Today.AddDays(days))
                .ToList()
                .ConvertAll(w => new WeatherForecast
                {
                    Date = w.AcquisitionDateTime,
                    TemperatureC = (int)w.WeatherDayInfo.Temperature,
                    Summary = w.Weathers?.FirstOrDefault()?.Description ?? string.Empty
                });
        }
    }
}