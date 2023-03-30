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
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get(int days = 8)
        {
            //Note: controller is tightly coupled to WeatherForecastService.
            var service = new WeatherForecastService();
            var result = await service.GetWeatherForecast(days);
            return Ok(result);
        }
    }
}