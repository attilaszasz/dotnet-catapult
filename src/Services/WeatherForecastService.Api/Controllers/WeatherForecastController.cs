using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Types;

namespace WeatherForecastService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _service;

        public WeatherForecastController(IWeatherForecastService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("/GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Post(WeatherForecastCriteria criteria, string supplierName = Constants.Suppliers.Dummy)
        {
            var result = await _service.GetWeatherForecast(criteria, supplierName);
            return Ok(result);
        }
    }
}