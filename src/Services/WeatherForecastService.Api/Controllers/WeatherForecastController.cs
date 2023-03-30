using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Types;

namespace WeatherForecastService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {
        }

        [HttpGet]
        [Route("/GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get(double latitude = 46.542679, double longitude = 24.557859, int days = 8, string supplierName = "Dummy")
        {
            //Note: controller is tightly coupled to WeatherForecastService.
            var service = new WeatherForecastService();
            var result = await service.GetWeatherForecast(latitude, longitude, days, supplierName);
            return Ok(result);
        }
    }
}