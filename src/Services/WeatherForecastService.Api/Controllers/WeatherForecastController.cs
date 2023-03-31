using Dummy;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenWeather;
using Types;

namespace WeatherForecastService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _service;

        public WeatherForecastController()
        {
            //Note: controller is tightly coupled to WeatherForecastService and suppliers
            _service = new WeatherForecastService(
                        dummy: new DummyWeatherSupplier(),
                        openWeather: new OpenWeatherSupplier(new OpenWeatherAdapter(new ConfigurationBuilder().AddUserSecrets("7b91147d-502c-4fa9-b973-294be01c474b").Build())));
        }

        [HttpPost]
        [Route("/GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get(WeatherForecastCriteria criteria, string supplierName = "Dummy")
        {
            //Note: controller is tightly coupled to WeatherForecastService.
            var result = await _service.GetWeatherForecast(criteria, supplierName);
            return Ok(result);
        }
    }
}