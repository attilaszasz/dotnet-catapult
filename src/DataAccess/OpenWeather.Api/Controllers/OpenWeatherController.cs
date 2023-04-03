using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Types;

namespace OpenWeather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenWeatherController : ControllerBase
    {
        private readonly IWeatherSupplier _supplier;

        public OpenWeatherController(IWeatherSupplier supplier)
        {
            _supplier = supplier;
        }

        [HttpPost]
        [Route("/GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Post(WeatherForecastCriteria criteria)
        {
            var result = await _supplier.GetWeatherForecast(criteria);
            return Ok(result);
        }
    }
}